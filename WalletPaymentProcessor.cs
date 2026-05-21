namespace OnlineShoppingSystem;

/// <summary>
/// Processes checkout payments by debiting the customer's wallet balance.
/// </summary>
public sealed class WalletPaymentProcessor : IPaymentProcessor
{
    private readonly AppDataStore _store;

    public WalletPaymentProcessor(AppDataStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public Payment ProcessPayment(Customer customer, Order order)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(order);

        var paymentSucceeded = customer.TryDebitWallet(order.Total);
        var status = paymentSucceeded ? PaymentStatus.Successful : PaymentStatus.Failed;
        var message = paymentSucceeded ? "Payment completed." : "Insufficient wallet balance.";
        var payment = new Payment(_store.GetNextPaymentId(), order.Id, customer.Id, order.Total, status, message);

        _store.Payments.Add(payment);
        return payment;
    }
}
