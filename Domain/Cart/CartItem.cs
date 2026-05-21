namespace OnlineShoppingSystem;

/// <summary>
/// Represents a product and quantity selected by a customer before checkout.
/// </summary>
public sealed class CartItem
{
    public CartItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        Product = product;
        UpdateQuantity(quantity);
    }

    public Product Product { get; }

    public int Quantity { get; private set; }

    public decimal LineTotal => Product.Price * Quantity;

    /// <summary>
    /// Updates the selected quantity for this cart item.
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        Quantity = quantity;
    }
}
