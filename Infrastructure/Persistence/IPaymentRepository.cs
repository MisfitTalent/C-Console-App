namespace OnlineShoppingSystem;

/// <summary>
/// Defines persistence operations for payments.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Loads payments from persistent storage.
    /// </summary>
    IReadOnlyCollection<Payment> LoadPayments();

    /// <summary>
    /// Saves payments to persistent storage.
    /// </summary>
    void SavePayments(IEnumerable<Payment> payments);
}
