namespace OnlineShoppingSystem;

/// <summary>
/// Defines product catalog, inventory, and review operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Adds a product to the catalog.
    /// </summary>
    Product AddProduct(string name, string description, string category, decimal price, int stockQuantity, int reorderLevel);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    bool UpdateProduct(int productId, string name, string description, string category, decimal price, int stockQuantity, int reorderLevel);

    /// <summary>
    /// Deletes a product from the catalog.
    /// </summary>
    bool DeleteProduct(int productId);

    /// <summary>
    /// Adds stock to an existing product.
    /// </summary>
    bool RestockProduct(int productId, int quantity);

    /// <summary>
    /// Returns all products in the catalog.
    /// </summary>
    IReadOnlyCollection<Product> GetProducts();

    /// <summary>
    /// Finds a product by identifier.
    /// </summary>
    Product? GetProductById(int productId);

    /// <summary>
    /// Searches products by name, description, or category.
    /// </summary>
    IReadOnlyCollection<Product> SearchProducts(string searchTerm);

    /// <summary>
    /// Returns products that belong to a category.
    /// </summary>
    IReadOnlyCollection<Product> GetProductsByCategory(string category);

    /// <summary>
    /// Returns products within the supplied price range.
    /// </summary>
    IReadOnlyCollection<Product> GetProductsByPriceRange(decimal minimumPrice, decimal maximumPrice);

    /// <summary>
    /// Returns products with at least the supplied average rating.
    /// </summary>
    IReadOnlyCollection<Product> GetProductsByMinimumRating(decimal minimumRating);

    /// <summary>
    /// Returns products that currently have stock available.
    /// </summary>
    IReadOnlyCollection<Product> GetInStockProducts();

    /// <summary>
    /// Returns products at or below their reorder level.
    /// </summary>
    IReadOnlyCollection<Product> GetLowStockProducts();

    /// <summary>
    /// Adds a review to a product.
    /// </summary>
    Review AddReview(Customer customer, int productId, int rating, string comment);
}
