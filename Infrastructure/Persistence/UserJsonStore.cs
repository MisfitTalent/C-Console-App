using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShoppingSystem;

/// <summary>
/// Persists application users to a local JSON file.
/// </summary>
public sealed class UserJsonStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;

    public UserJsonStore(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Loads users from the configured JSON file.
    /// </summary>
    public IReadOnlyCollection<User> LoadUsers()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        var storedUsers = JsonSerializer.Deserialize<List<StoredUser>>(json, JsonOptions) ?? [];
        return storedUsers.Select(storedUser => storedUser.ToUser()).ToList();
    }

    /// <summary>
    /// Saves users to the configured JSON file.
    /// </summary>
    public void SaveUsers(IEnumerable<User> users)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var storedUsers = users.Select(StoredUser.FromUser).ToList();
        var json = JsonSerializer.Serialize(storedUsers, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
