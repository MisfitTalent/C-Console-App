namespace OnlineShoppingSystem;

/// <summary>
/// Handles user registration and authentication against the in-memory store.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly AppDataStore _store;

    public UserService(AppDataStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public User Register(string name, string username, string password, UserRole role)
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

        var normalizedUsername = username.Trim();
        var usernameExists = _store.Users.Any(user =>
            user.Username.Equals(normalizedUsername, StringComparison.OrdinalIgnoreCase));

        if (usernameExists)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        User user = role == UserRole.Administrator
            ? new Administrator(_store.GetNextUserId(), name.Trim(), normalizedUsername, password)
            : new Customer(_store.GetNextUserId(), name.Trim(), normalizedUsername, password);

        _store.Users.Add(user);
        return user;
    }

    /// <inheritdoc />
    public User? Login(string username, string password)
    {
        return _store.Users.FirstOrDefault(user =>
            user.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)
            && user.Password == password);
    }
}
