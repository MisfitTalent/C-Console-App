namespace OnlineShoppingSystem;

/// <summary>
/// Represents an administrator who manages products, inventory, orders, and reports.
/// </summary>
public sealed class Administrator : User
{
    public Administrator(int id, string name, string username, string password)
        : base(id, name, username, password, UserRole.Administrator)
    {
    }
}
