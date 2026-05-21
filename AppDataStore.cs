namespace OnlineShoppingSystem;

/// <summary>
/// Stores application data in memory for the console simulation.
/// </summary>
public sealed class AppDataStore
{
    private int _nextUserId = 1;
    private int _nextProductId = 1;
    private int _nextOrderId = 1;
    private int _nextPaymentId = 1;
    private int _nextReviewId = 1;

    public AppDataStore()
    {
        Seed();
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
    /// Returns the next unique product identifier.
    /// </summary>
    public int GetNextProductId() => _nextProductId++;

    /// <summary>
    /// Returns the next unique order identifier.
    /// </summary>
    public int GetNextOrderId() => _nextOrderId++;

    /// <summary>
    /// Returns the next unique payment identifier.
    /// </summary>
    public int GetNextPaymentId() => _nextPaymentId++;

    /// <summary>
    /// Returns the next unique review identifier.
    /// </summary>
    public int GetNextReviewId() => _nextReviewId++;

    private void Seed()
    {
        var admin = new Administrator(GetNextUserId(), "System Admin", "admin", "admin123");
        var customer = new Customer(GetNextUserId(), "Demo Customer", "customer", "customer123");
        customer.AddWalletFunds(1_500);

        Users.Add(admin);
        Users.Add(customer);

        Products.Add(new Product(GetNextProductId(), "Laptop", "Portable work machine", "Electronics", 899.99m, 8, 2));
        Products.Add(new Product(GetNextProductId(), "Wireless Mouse", "Bluetooth mouse", "Electronics", 29.99m, 25, 5));
        Products.Add(new Product(GetNextProductId(), "Coffee Beans", "Premium roasted beans", "Groceries", 14.50m, 40, 10));
        Products.Add(new Product(GetNextProductId(), "Desk Chair", "Ergonomic office chair", "Furniture", 179.00m, 5, 2));
    }
}
