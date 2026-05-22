namespace OnlineShoppingSystem;

/// <summary>
/// Defines persistence operations for users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Loads users from persistent storage.
    /// </summary>
    IReadOnlyCollection<User> LoadUsers();

    /// <summary>
    /// Saves users to persistent storage.
    /// </summary>
    void SaveUsers(IEnumerable<User> users);
}
