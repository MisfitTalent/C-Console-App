using OnlineShoppingSystem;

var tests = new (string Name, Action Test)[]
{
    ("Cart calculates totals and supports quantity updates", CartCalculatesTotals),
    ("Product service filters products with LINQ queries", ProductServiceFiltersProducts),
    ("User service rejects duplicate usernames", UserServiceRejectsDuplicateUsernames),
    ("Wallet payment fails when balance is too low", WalletPaymentFailsWithInsufficientBalance),
    ("Checkout creates order, payment, and stock changes", CheckoutCreatesOrderPaymentAndStockChanges)
};

var failures = new List<string>();
foreach (var (name, test) in tests)
{
    try
    {
        RunInIsolatedDataDirectory(test);
        Console.WriteLine($"PASS: {name}");
    }
    catch (Exception exception)
    {
        failures.Add($"{name}: {exception.Message}");
        Console.WriteLine($"FAIL: {name}");
        Console.WriteLine($"      {exception.Message}");
    }
}

if (failures.Count > 0)
{
    Console.WriteLine();
    Console.WriteLine("Failed tests:");
    foreach (var failure in failures)
    {
        Console.WriteLine($"- {failure}");
    }

    Environment.Exit(1);
}

Console.WriteLine();
Console.WriteLine("All automated tests passed.");

static void CartCalculatesTotals()
{
    var product = new Product(1, "Notebook", "Paper notebook", "Stationery", 10m, 5, 1);
    var cart = new Cart();

    cart.AddProduct(product, 2);
    AssertEqual(20m, cart.Total, "Cart total should include added item quantity.");

    cart.UpdateProductQuantity(product.Id, 3);
    AssertEqual(30m, cart.Total, "Cart total should update after quantity changes.");

    cart.RemoveProduct(product.Id);
    AssertTrue(cart.IsEmpty, "Cart should be empty after removing the only product.");
}

static void ProductServiceFiltersProducts()
{
    var store = new AppDataStore();
    var productService = new ProductService(store);

    var electronics = productService.GetProductsByCategory("Electronics");
    AssertTrue(electronics.All(product => product.Category == "Electronics"), "Category filter should only return electronics.");

    var affordableProducts = productService.GetProductsByPriceRange(1m, 30m);
    AssertTrue(affordableProducts.All(product => product.Price is >= 1m and <= 30m), "Price filter should respect min and max values.");

    var inStockProducts = productService.GetInStockProducts();
    AssertTrue(inStockProducts.All(product => product.StockQuantity > 0), "In-stock filter should exclude unavailable products.");
}

static void UserServiceRejectsDuplicateUsernames()
{
    var store = new AppDataStore();
    var userService = new UserService(store, store.UserFactory);

    userService.Register("Test User", "duplicate", "pass123", UserRole.Customer);

    AssertThrows<InvalidOperationException>(
        () => userService.Register("Other User", "DUPLICATE", "pass123", UserRole.Customer),
        "Duplicate username registration should fail case-insensitively.");
}

static void WalletPaymentFailsWithInsufficientBalance()
{
    var store = new AppDataStore();
    var paymentProcessor = new WalletPaymentProcessor(store);
    var customer = new Customer(99, "Low Balance", "low.balance", "pass123");
    var order = new Order(1, customer.Id, [new OrderItem(1, "Laptop", 899.99m, 1)]);

    var payment = paymentProcessor.ProcessPayment(customer, order);

    AssertEqual(PaymentStatus.Failed, payment.Status, "Payment should fail when wallet balance is too low.");
    AssertEqual(1, store.Payments.Count, "Failed payment attempts should still be recorded.");
}

static void CheckoutCreatesOrderPaymentAndStockChanges()
{
    var store = new AppDataStore();
    var productService = new ProductService(store);
    var paymentProcessor = new WalletPaymentProcessor(store);
    var orderService = new OrderService(store, paymentProcessor);
    var customer = new Customer(99, "Checkout User", "checkout", "pass123");
    customer.AddWalletFunds(2_000m);

    var product = productService.GetProducts().First();
    var startingStock = product.StockQuantity;
    customer.Cart.AddProduct(product, 1);

    var order = orderService.PlaceOrder(customer);
    var payment = orderService.GetPaymentForOrder(order.Id);

    AssertTrue(customer.Cart.IsEmpty, "Cart should be cleared after checkout.");
    AssertEqual(startingStock - 1, product.StockQuantity, "Checkout should reduce product stock.");
    AssertTrue(store.Orders.Any(storedOrder => storedOrder.Id == order.Id), "Placed order should be stored.");
    AssertTrue(payment is not null && payment.Status == PaymentStatus.Successful, "Successful checkout should create a successful payment.");
}

static void RunInIsolatedDataDirectory(Action test)
{
    var originalDirectory = Directory.GetCurrentDirectory();
    var testDirectory = Path.Combine(Path.GetTempPath(), $"online-shopping-tests-{Guid.NewGuid():N}");
    Directory.CreateDirectory(testDirectory);

    try
    {
        Directory.SetCurrentDirectory(testDirectory);
        test();
    }
    finally
    {
        Directory.SetCurrentDirectory(originalDirectory);
        Directory.Delete(testDirectory, recursive: true);
    }
}

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

static void AssertEqual<T>(T expected, T actual, string message)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"{message} Expected: {expected}. Actual: {actual}.");
    }
}

static void AssertThrows<TException>(Action action, string message)
    where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException(message);
}
