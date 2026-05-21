namespace OnlineShoppingSystem;

/// <summary>
/// Defines payment behavior for order checkout.
/// </summary>
public interface IPaymentProcessor
{
    /// <summary>
    /// Processes payment for an order.
    /// </summary>
    Payment ProcessPayment(Customer customer, Order order);
}
