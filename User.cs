namespace OnlineShoppingSystem;

/// <summary>
/// Represents a registered system user with shared identity and login details.
/// </summary>
public abstract class User
{
    protected User(int id, string name, string username, string password, UserRole role)
    {
        Id = id;
        Name = name;
        Username = username;
        Password = password;
        Role = role;
    }

    public int Id { get; }

    public string Name { get; private set; }

    public string Username { get; }

    public string Password { get; }

    public UserRole Role { get; }

    /// <summary>
    /// Updates the display name for the user.
    /// </summary>
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        Name = name.Trim();
    }
}
