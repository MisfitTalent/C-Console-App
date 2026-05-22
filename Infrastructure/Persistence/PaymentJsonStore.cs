using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShoppingSystem;

/// <summary>
/// Persists payments to a local JSON file.
/// </summary>
public sealed class PaymentJsonStore : IPaymentRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;

    public PaymentJsonStore(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Loads payments from the configured JSON file.
    /// </summary>
    public IReadOnlyCollection<Payment> LoadPayments()
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

        var storedPayments = JsonSerializer.Deserialize<List<StoredPayment>>(json, JsonOptions) ?? [];
        return storedPayments.Select(storedPayment => storedPayment.ToPayment()).ToList();
    }

    /// <summary>
    /// Saves payments to the configured JSON file.
    /// </summary>
    public void SavePayments(IEnumerable<Payment> payments)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var storedPayments = payments.Select(StoredPayment.FromPayment).ToList();
        var json = JsonSerializer.Serialize(storedPayments, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
