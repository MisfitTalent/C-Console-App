using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShoppingSystem;

/// <summary>
/// Persists orders to a local JSON file.
/// </summary>
public sealed class OrderJsonStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;

    public OrderJsonStore(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Loads orders from the configured JSON file.
    /// </summary>
    public IReadOnlyCollection<Order> LoadOrders()
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

        var storedOrders = JsonSerializer.Deserialize<List<StoredOrder>>(json, JsonOptions) ?? [];
        return storedOrders.Select(storedOrder => storedOrder.ToOrder()).ToList();
    }

    /// <summary>
    /// Saves orders to the configured JSON file.
    /// </summary>
    public void SaveOrders(IEnumerable<Order> orders)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var storedOrders = orders.Select(StoredOrder.FromOrder).ToList();
        var json = JsonSerializer.Serialize(storedOrders, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
