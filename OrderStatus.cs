namespace OnlineShoppingSystem;

/// <summary>
/// Defines the lifecycle states available for customer orders.
/// </summary>
public enum OrderStatus
{
    Pending = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}
