namespace OnlineShoppingSystem;

/// <summary>
/// Defines user registration and login operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user with the selected role.
    /// </summary>
    User Register(string name, string username, string password, UserRole role);

    /// <summary>
    /// Authenticates a user by username and password.
    /// </summary>
    User? Login(string username, string password);

    /// <summary>
    /// Persists the current user list to storage.
    /// </summary>
    void SaveUsers();
}
