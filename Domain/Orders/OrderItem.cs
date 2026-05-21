namespace OnlineShoppingSystem;

/// <summary>
/// Represents the immutable product snapshot stored on an order.
/// </summary>
public sealed class OrderItem
{
    public OrderItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public int ProductId { get; }

    public string ProductName { get; }

    public decimal UnitPrice { get; }

    public int Quantity { get; }

    public decimal LineTotal => UnitPrice * Quantity;
}
