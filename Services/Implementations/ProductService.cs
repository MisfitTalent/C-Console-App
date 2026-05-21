namespace OnlineShoppingSystem;

/// <summary>
/// Manages product catalog operations, inventory, searching, and reviews.
/// </summary>
public sealed class ProductService : IProductService
{
    private readonly AppDataStore _store;

    public ProductService(AppDataStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public Product AddProduct(string name, string description, string category, decimal price, int stockQuantity, int reorderLevel)
    {
        var product = new Product(_store.GetNextProductId(), name, description, category, price, stockQuantity, reorderLevel);
        _store.Products.Add(product);
        return product;
    }

    /// <inheritdoc />
    public bool UpdateProduct(int productId, string name, string description, string category, decimal price, int stockQuantity, int reorderLevel)
    {
        var product = GetProductById(productId);
        if (product is null)
        {
            return false;
        }

        product.UpdateDetails(name, description, category, price, stockQuantity, reorderLevel);
        return true;
    }

    /// <inheritdoc />
    public bool DeleteProduct(int productId)
    {
        var product = GetProductById(productId);
        return product is not null && _store.Products.Remove(product);
    }

    /// <inheritdoc />
    public bool RestockProduct(int productId, int quantity)
    {
        var product = GetProductById(productId);
        if (product is null)
        {
            return false;
        }

        product.Restock(quantity);
        return true;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Product> GetProducts()
    {
        return _store.Products
            .OrderBy(product => product.Category)
            .ThenBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public Product? GetProductById(int productId)
    {
        return _store.Products.FirstOrDefault(product => product.Id == productId);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Product> SearchProducts(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetProducts();
        }

        var normalizedSearchTerm = searchTerm.Trim();
        return _store.Products
            .Where(product =>
                product.Name.Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase)
                || product.Description.Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase)
                || product.Category.Contains(normalizedSearchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Product> GetLowStockProducts()
    {
        return _store.Products
            .Where(product => product.IsLowStock)
            .OrderBy(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public Review AddReview(Customer customer, int productId, int rating, string comment)
    {
        ArgumentNullException.ThrowIfNull(customer);

        var product = GetProductById(productId);
        if (product is null)
        {
            throw new InvalidOperationException("Product was not found.");
        }

        var review = new Review(_store.GetNextReviewId(), customer.Id, productId, rating, comment);
        product.AddReview(review);
        return review;
    }
}
