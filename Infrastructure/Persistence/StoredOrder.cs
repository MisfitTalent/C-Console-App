namespace OnlineShoppingSystem;

/// <summary>
/// Represents the JSON-friendly shape used to persist orders.
/// </summary>
public sealed class StoredOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public List<StoredOrderItem> Items { get; set; } = [];

    /// <summary>
    /// Creates a stored order from a runtime order.
    /// </summary>
    public static StoredOrder FromOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new StoredOrder
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            Items = order.Items.Select(StoredOrderItem.FromOrderItem).ToList()
        };
    }

    /// <summary>
    /// Creates a runtime order from this stored order.
    /// </summary>
    public Order ToOrder()
    {
        var items = Items.Select(item => item.ToOrderItem()).ToList();
        return new Order(Id, CustomerId, items, CreatedAt, Status);
    }
}

/// <summary>
/// Represents a persisted order item linked to a product by identifier.
/// </summary>
public sealed class StoredOrderItem
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    /// <summary>
    /// Creates a stored order item from a runtime order item.
    /// </summary>
    public static StoredOrderItem FromOrderItem(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        return new StoredOrderItem
        {
            ProductId = orderItem.ProductId,
            ProductName = orderItem.ProductName,
            UnitPrice = orderItem.UnitPrice,
            Quantity = orderItem.Quantity
        };
    }

    /// <summary>
    /// Creates a runtime order item from this stored order item.
    /// </summary>
    public OrderItem ToOrderItem()
    {
        return new OrderItem(ProductId, ProductName, UnitPrice, Quantity);
    }
}
