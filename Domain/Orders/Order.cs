namespace OnlineShoppingSystem;

/// <summary>
/// Represents a placed customer order and its fulfillment status.
/// </summary>
public sealed class Order
{
    public Order(int id, int customerId, IReadOnlyCollection<OrderItem> items)
        : this(id, customerId, items, DateTime.Now, OrderStatus.Pending)
    {
    }

    public Order(int id, int customerId, IReadOnlyCollection<OrderItem> items, DateTime createdAt, OrderStatus status)
    {
        if (items.Count == 0)
        {
            throw new ArgumentException("An order must contain at least one item.", nameof(items));
        }

        Id = id;
        CustomerId = customerId;
        Items = items;
        CreatedAt = createdAt;
        Status = status;
    }

    public int Id { get; }

    public int CustomerId { get; }

    public IReadOnlyCollection<OrderItem> Items { get; }

    public DateTime CreatedAt { get; }

    public OrderStatus Status { get; private set; }

    public decimal Total => Items.Sum(item => item.LineTotal);

    /// <summary>
    /// Updates the fulfillment status for this order.
    /// </summary>
    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }
}
