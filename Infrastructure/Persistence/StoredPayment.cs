namespace OnlineShoppingSystem;

/// <summary>
/// Represents the JSON-friendly shape used to persist payments.
/// </summary>
public sealed class StoredPayment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime PaidAt { get; set; }

    /// <summary>
    /// Creates a stored payment from a runtime payment.
    /// </summary>
    public static StoredPayment FromPayment(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        return new StoredPayment
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            CustomerId = payment.CustomerId,
            Amount = payment.Amount,
            Status = payment.Status,
            Message = payment.Message,
            PaidAt = payment.PaidAt
        };
    }

    /// <summary>
    /// Creates a runtime payment from this stored payment.
    /// </summary>
    public Payment ToPayment()
    {
        return new Payment(Id, OrderId, CustomerId, Amount, Status, Message, PaidAt);
    }
}
