namespace OnlineShoppingSystem;

/// <summary>
/// Handles user registration and authentication against the in-memory store.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly AppDataStore _store;
    private readonly IUserFactory _userFactory;

    public UserService(AppDataStore store, IUserFactory userFactory)
    {
        _store = store;
        _userFactory = userFactory;
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

        var user = _userFactory.Create(_store.GetNextUserId(), name, normalizedUsername, password, role);

        _store.Users.Add(user);
        _store.SaveUsers();
        return user;
    }

    /// <inheritdoc />
    public User? Login(string username, string password)
    {
        return _store.Users.FirstOrDefault(user =>
            user.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)
            && user.Password == password);
    }

    /// <inheritdoc />
    public void SaveUsers()
    {
        _store.SaveUsers();
    }
}
