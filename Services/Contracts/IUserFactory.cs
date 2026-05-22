namespace OnlineShoppingSystem;

/// <summary>
/// Defines user creation so role-specific user construction stays in one place.
/// </summary>
public interface IUserFactory
{
    /// <summary>
    /// Creates the correct user subtype for the supplied role.
    /// </summary>
    User Create(int id, string name, string username, string password, UserRole role, decimal walletBalance = 0);
}
