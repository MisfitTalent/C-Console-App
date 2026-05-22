namespace OnlineShoppingSystem;

/// <summary>
/// Defines persistence operations for products and their reviews.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Loads products from persistent storage.
    /// </summary>
    IReadOnlyCollection<Product> LoadProducts();

    /// <summary>
    /// Saves products to persistent storage.
    /// </summary>
    void SaveProducts(IEnumerable<Product> products);
}
