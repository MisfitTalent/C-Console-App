namespace OnlineShoppingSystem;

/// <summary>
/// Stores application data in memory for the console simulation.
/// </summary>
public sealed class AppDataStore
{
    private readonly UserJsonStore _userJsonStore;
    private readonly ProductJsonStore _productJsonStore;
    private readonly OrderJsonStore _orderJsonStore;
    private readonly PaymentJsonStore _paymentJsonStore;
    private int _nextUserId = 1;
    private int _nextProductId = 1;
    private int _nextOrderId = 1;
    private int _nextPaymentId = 1;
    private int _nextReviewId = 1;

    public AppDataStore()
    {
        var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        _userJsonStore = new UserJsonStore(Path.Combine(dataDirectory, "Users.json"));
        _productJsonStore = new ProductJsonStore(Path.Combine(dataDirectory, "Products.json"));
        _orderJsonStore = new OrderJsonStore(Path.Combine(dataDirectory, "Orders.json"));
        _paymentJsonStore = new PaymentJsonStore(Path.Combine(dataDirectory, "Payments.json"));

        LoadProducts();
        LoadUsers();
        LoadOrders();
        LoadPayments();
    }

    public List<User> Users { get; } = [];

    public List<Product> Products { get; } = [];

    public List<Order> Orders { get; } = [];

    public List<Payment> Payments { get; } = [];

    /// <summary>
    /// Returns the next unique user identifier.
    /// </summary>
    public int GetNextUserId() => _nextUserId++;

    /// <summary>
    /// Saves all current users to persistent storage.
    /// </summary>
    public void SaveUsers()
    {
        _userJsonStore.SaveUsers(Users);
    }

    /// <summary>
    /// Saves all current products to persistent storage.
    /// </summary>
    public void SaveProducts()
    {
        _productJsonStore.SaveProducts(Products);
    }

    /// <summary>
    /// Returns the next unique product identifier.
    /// </summary>
    public int GetNextProductId() => _nextProductId++;

    /// <summary>
    /// Returns the next unique order identifier.
    /// </summary>
    public int GetNextOrderId() => _nextOrderId++;

    /// <summary>
    /// Saves all current orders to persistent storage.
    /// </summary>
    public void SaveOrders()
    {
        _orderJsonStore.SaveOrders(Orders);
    }

    /// <summary>
    /// Returns the next unique payment identifier.
    /// </summary>
    public int GetNextPaymentId() => _nextPaymentId++;

    /// <summary>
    /// Saves all current payments to persistent storage.
    /// </summary>
    public void SavePayments()
    {
        _paymentJsonStore.SavePayments(Payments);
    }

    /// <summary>
    /// Returns the next unique review identifier.
    /// </summary>
    public int GetNextReviewId() => _nextReviewId++;

    private void LoadProducts()
    {
        var products = _productJsonStore.LoadProducts();
        if (products.Count == 0)
        {
            SeedProducts();
            SaveProducts();
        }
        else
        {
            Products.AddRange(products);
        }

        _nextProductId = Products.Count == 0 ? 1 : Products.Max(product => product.Id) + 1;
        _nextReviewId = Products
            .SelectMany(product => product.Reviews)
            .Select(review => review.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }

    private void LoadUsers()
    {
        var users = _userJsonStore.LoadUsers();
        if (users.Count == 0)
        {
            SeedUsers();
            SaveUsers();
        }
        else
        {
            Users.AddRange(users);
        }

        _nextUserId = Users.Count == 0 ? 1 : Users.Max(user => user.Id) + 1;
    }

    private void LoadOrders()
    {
        var orders = _orderJsonStore.LoadOrders();
        Orders.AddRange(orders);
        _nextOrderId = Orders.Count == 0 ? 1 : Orders.Max(order => order.Id) + 1;
    }

    private void LoadPayments()
    {
        var payments = _paymentJsonStore.LoadPayments();
        Payments.AddRange(payments);
        _nextPaymentId = Payments.Count == 0 ? 1 : Payments.Max(payment => payment.Id) + 1;
    }

    private void SeedUsers()
    {
        var admin = new Administrator(GetNextUserId(), "System Admin", "admin", "admin123");
        var customer = new Customer(GetNextUserId(), "Demo Customer", "customer", "customer123");
        customer.AddWalletFunds(1_500);

        Users.Add(admin);
        Users.Add(customer);
    }

    private void SeedProducts()
    {
        Products.Add(new Product(GetNextProductId(), "Laptop", "Portable work machine", "Electronics", 899.99m, 8, 2));
        Products.Add(new Product(GetNextProductId(), "Wireless Mouse", "Bluetooth mouse", "Electronics", 29.99m, 25, 5));
        Products.Add(new Product(GetNextProductId(), "Coffee Beans", "Premium roasted beans", "Groceries", 14.50m, 40, 10));
        Products.Add(new Product(GetNextProductId(), "Desk Chair", "Ergonomic office chair", "Furniture", 179.00m, 5, 2));
    }
}
