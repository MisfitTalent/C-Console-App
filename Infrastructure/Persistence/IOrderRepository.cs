namespace OnlineShoppingSystem;

/// <summary>
/// Defines persistence operations for orders.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Loads orders from persistent storage.
    /// </summary>
    IReadOnlyCollection<Order> LoadOrders();

    /// <summary>
    /// Saves orders to persistent storage.
    /// </summary>
    void SaveOrders(IEnumerable<Order> orders);
}
