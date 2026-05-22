using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShoppingSystem;

/// <summary>
/// Persists products and their reviews to a local JSON file.
/// </summary>
public sealed class ProductJsonStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;

    public ProductJsonStore(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Loads products from the configured JSON file.
    /// </summary>
    public IReadOnlyCollection<Product> LoadProducts()
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

        var storedProducts = JsonSerializer.Deserialize<List<StoredProduct>>(json, JsonOptions) ?? [];
        return storedProducts.Select(storedProduct => storedProduct.ToProduct()).ToList();
    }

    /// <summary>
    /// Saves products to the configured JSON file.
    /// </summary>
    public void SaveProducts(IEnumerable<Product> products)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var storedProducts = products.Select(StoredProduct.FromProduct).ToList();
        var json = JsonSerializer.Serialize(storedProducts, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
