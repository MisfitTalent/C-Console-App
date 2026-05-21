namespace OnlineShoppingSystem;

/// <summary>
/// Represents the customer's current shopping cart.
/// </summary>
public sealed class Cart
{
    private readonly List<CartItem> _items = [];

    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public bool IsEmpty => _items.Count == 0;

    public decimal Total => _items.Sum(item => item.LineTotal);

    /// <summary>
    /// Adds a product to the cart or increases its selected quantity.
    /// </summary>
    public void AddProduct(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        var existingItem = _items.FirstOrDefault(item => item.Product.Id == product.Id);
        if (existingItem is null)
        {
            _items.Add(new CartItem(product, quantity));
            return;
        }

        existingItem.UpdateQuantity(existingItem.Quantity + quantity);
    }

    /// <summary>
    /// Updates the quantity of a product already present in the cart.
    /// </summary>
    public bool UpdateProductQuantity(int productId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        var item = _items.FirstOrDefault(cartItem => cartItem.Product.Id == productId);
        if (item is null)
        {
            return false;
        }

        item.UpdateQuantity(quantity);
        return true;
    }

    /// <summary>
    /// Removes a product from the cart.
    /// </summary>
    public bool RemoveProduct(int productId)
    {
        var item = _items.FirstOrDefault(cartItem => cartItem.Product.Id == productId);
        return item is not null && _items.Remove(item);
    }

    /// <summary>
    /// Empties the cart after checkout or user action.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }
}
