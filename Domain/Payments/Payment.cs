namespace OnlineShoppingSystem;

/// <summary>
/// Represents the result of a simulated wallet payment for an order.
/// </summary>
public sealed class Payment
{
    public Payment(int id, int orderId, int customerId, decimal amount, PaymentStatus status, string message)
        : this(id, orderId, customerId, amount, status, message, DateTime.Now)
    {
    }

    public Payment(int id, int orderId, int customerId, decimal amount, PaymentStatus status, string message, DateTime paidAt)
    {
        Id = id;
        OrderId = orderId;
        CustomerId = customerId;
        Amount = amount;
        Status = status;
        Message = message;
        PaidAt = paidAt;
    }

    public int Id { get; }

    public int OrderId { get; }

    public int CustomerId { get; }

    public decimal Amount { get; }

    public PaymentStatus Status { get; }

    public string Message { get; }

    public DateTime PaidAt { get; }
}
