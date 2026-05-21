namespace OnlineShoppingSystem;

/// <summary>
/// Provides all customer console operations for shopping and order tracking.
/// </summary>
public sealed class CustomerMenu
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public CustomerMenu(IProductService productService, IOrderService orderService)
    {
        _productService = productService;
        _orderService = orderService;
    }

    /// <summary>
    /// Shows the customer menu until the user logs out.
    /// </summary>
    public void Show(Customer customer)
    {
        while (true)
        {
            ConsoleRenderer.PrintHeader("Customer Menu");
            Console.WriteLine("1. Browse Products");
            Console.WriteLine("2. Search Products");
            Console.WriteLine("3. Add Product to Cart");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Update Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. View Wallet Balance");
            Console.WriteLine("8. Add Wallet Funds");
            Console.WriteLine("9. View Order History");
            Console.WriteLine("10. Track Orders");
            Console.WriteLine("11. Review Products");
            Console.WriteLine("12. Logout");

            var choice = ConsoleInput.ReadInt("Select option: ", 1, 12);
            if (choice == 12)
            {
                return;
            }

            HandleChoice(customer, choice);
        }
    }

    private void HandleChoice(Customer customer, int choice)
    {
        try
        {
            switch (choice)
            {
                case 1:
                    BrowseProducts();
                    break;
                case 2:
                    SearchProducts();
                    break;
                case 3:
                    AddProductToCart(customer);
                    break;
                case 4:
                    ViewCart(customer);
                    break;
                case 5:
                    UpdateCart(customer);
                    break;
                case 6:
                    Checkout(customer);
                    break;
                case 7:
                    ViewWalletBalance(customer);
                    break;
                case 8:
                    AddWalletFunds(customer);
                    break;
                case 9:
                    ViewOrderHistory(customer);
                    break;
                case 10:
                    TrackOrders(customer);
                    break;
                case 11:
                    ReviewProduct(customer);
                    break;
            }
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            Console.WriteLine(exception.Message);
        }
        finally
        {
            ConsoleInput.WaitForMenuReturn();
        }
    }

    private void BrowseProducts()
    {
        ConsoleRenderer.PrintHeader("Products");
        ConsoleRenderer.PrintProducts(_productService.GetProducts());
    }

    private void SearchProducts()
    {
        var searchTerm = ConsoleInput.ReadRequiredText("Search: ");
        ConsoleRenderer.PrintProducts(_productService.SearchProducts(searchTerm));
    }

    private void AddProductToCart(Customer customer)
    {
        ConsoleRenderer.PrintProducts(_productService.GetProducts());
        var productId = ConsoleInput.ReadInt("Product ID: ", 1, int.MaxValue);
        var quantity = ConsoleInput.ReadInt("Quantity: ", 1, int.MaxValue);
        var product = _productService.GetProductById(productId);

        if (product is null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        if (quantity > product.StockQuantity)
        {
            Console.WriteLine("Requested quantity exceeds available stock.");
            return;
        }

        customer.Cart.AddProduct(product, quantity);
        Console.WriteLine("Product added to cart.");
    }

    private static void ViewCart(Customer customer)
    {
        ConsoleRenderer.PrintHeader("Cart");
        ConsoleRenderer.PrintCart(customer.Cart);
    }

    private static void UpdateCart(Customer customer)
    {
        ConsoleRenderer.PrintCart(customer.Cart);
        if (customer.Cart.IsEmpty)
        {
            return;
        }

        var productId = ConsoleInput.ReadInt("Product ID: ", 1, int.MaxValue);
        Console.WriteLine("1. Change quantity");
        Console.WriteLine("2. Remove product");
        var choice = ConsoleInput.ReadInt("Select option: ", 1, 2);

        if (choice == 2)
        {
            Console.WriteLine(customer.Cart.RemoveProduct(productId) ? "Product removed." : "Product not found in cart.");
            return;
        }

        var quantity = ConsoleInput.ReadInt("New quantity: ", 1, int.MaxValue);
        Console.WriteLine(customer.Cart.UpdateProductQuantity(productId, quantity)
            ? "Cart updated."
            : "Product not found in cart.");
    }

    private void Checkout(Customer customer)
    {
        ConsoleRenderer.PrintCart(customer.Cart);
        if (customer.Cart.IsEmpty)
        {
            return;
        }

        var order = _orderService.PlaceOrder(customer);
        Console.WriteLine($"Order {order.Id} placed successfully. Total: {order.Total:C}");
    }

    private static void ViewWalletBalance(Customer customer)
    {
        Console.WriteLine($"Wallet balance: {customer.WalletBalance:C}");
    }

    private static void AddWalletFunds(Customer customer)
    {
        var amount = ConsoleInput.ReadMoney("Amount: ");
        customer.AddWalletFunds(amount);
        Console.WriteLine($"Wallet balance: {customer.WalletBalance:C}");
    }

    private void ViewOrderHistory(Customer customer)
    {
        ConsoleRenderer.PrintOrders(_orderService.GetOrdersForCustomer(customer.Id));
    }

    private void TrackOrders(Customer customer)
    {
        var orders = _orderService.GetOrdersForCustomer(customer.Id);
        ConsoleRenderer.PrintOrders(orders);
    }

    private void ReviewProduct(Customer customer)
    {
        ConsoleRenderer.PrintProducts(_productService.GetProducts());
        var productId = ConsoleInput.ReadInt("Product ID: ", 1, int.MaxValue);
        var rating = ConsoleInput.ReadInt("Rating (1-5): ", 1, 5);
        var comment = ConsoleInput.ReadRequiredText("Comment: ");

        _productService.AddReview(customer, productId, rating, comment);
        Console.WriteLine("Review added.");
    }
}
