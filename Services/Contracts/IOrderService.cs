namespace OnlineShoppingSystem;

/// <summary>
/// Defines order placement, lookup, and status management operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Places an order for the customer's current cart.
    /// </summary>
    Order PlaceOrder(Customer customer);

    /// <summary>
    /// Returns all orders for a customer.
    /// </summary>
    IReadOnlyCollection<Order> GetOrdersForCustomer(int customerId);

    /// <summary>
    /// Returns all orders in the system.
    /// </summary>
    IReadOnlyCollection<Order> GetAllOrders();

    /// <summary>
    /// Returns all payments made by a customer.
    /// </summary>
    IReadOnlyCollection<Payment> GetPaymentsForCustomer(int customerId);

    /// <summary>
    /// Returns the payment linked to an order when one exists.
    /// </summary>
    Payment? GetPaymentForOrder(int orderId);

    /// <summary>
    /// Updates the status of an existing order.
    /// </summary>
    bool UpdateOrderStatus(int orderId, OrderStatus status);
}
