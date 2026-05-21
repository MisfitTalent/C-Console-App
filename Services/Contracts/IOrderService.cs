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
    /// Updates the status of an existing order.
    /// </summary>
    bool UpdateOrderStatus(int orderId, OrderStatus status);
}
