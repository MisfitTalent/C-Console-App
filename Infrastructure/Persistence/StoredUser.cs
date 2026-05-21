namespace OnlineShoppingSystem;

/// <summary>
/// Represents the JSON-friendly shape used to persist users between app sessions.
/// </summary>
public sealed class StoredUser
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public decimal WalletBalance { get; set; }

    /// <summary>
    /// Creates a stored user record from a runtime user object.
    /// </summary>
    public static StoredUser FromUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new StoredUser
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Password = user.Password,
            Role = user.Role,
            WalletBalance = user is Customer customer ? customer.WalletBalance : 0
        };
    }

    /// <summary>
    /// Creates the correct runtime user type from this stored record.
    /// </summary>
    public User ToUser()
    {
        if (Role == UserRole.Administrator)
        {
            return new Administrator(Id, Name, Username, Password);
        }

        var customer = new Customer(Id, Name, Username, Password);
        if (WalletBalance > 0)
        {
            customer.AddWalletFunds(WalletBalance);
        }

        return customer;
    }
}
