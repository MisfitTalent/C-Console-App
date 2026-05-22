namespace OnlineShoppingSystem;

/// <summary>
/// Creates role-specific user objects using the Factory design pattern.
/// </summary>
public sealed class UserFactory : IUserFactory
{
    /// <inheritdoc />
    public User Create(int id, string name, string username, string password, UserRole role, decimal walletBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }

        if (walletBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(walletBalance), "Wallet balance cannot be negative.");
        }

        return role switch
        {
            UserRole.Administrator => new Administrator(id, name.Trim(), username.Trim(), password),
            UserRole.Customer => CreateCustomer(id, name, username, password, walletBalance),
            _ => throw new ArgumentOutOfRangeException(nameof(role), "Unsupported user role.")
        };
    }

    private static Customer CreateCustomer(int id, string name, string username, string password, decimal walletBalance)
    {
        var customer = new Customer(id, name.Trim(), username.Trim(), password);
        if (walletBalance > 0)
        {
            customer.AddWalletFunds(walletBalance);
        }

        return customer;
    }
}
