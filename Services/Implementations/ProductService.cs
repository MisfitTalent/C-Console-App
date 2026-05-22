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
        _store.SaveProducts();
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
        _store.SaveProducts();
        return true;
    }

    /// <inheritdoc />
    public bool DeleteProduct(int productId)
    {
        var product = GetProductById(productId);
        if (product is null || !_store.Products.Remove(product))
        {
            return false;
        }

        _store.SaveProducts();
        return true;
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
        _store.SaveProducts();
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
    public IReadOnlyCollection<Product> GetProductsByCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return GetProducts();
        }

        return _store.Products
            .Where(product => product.Category.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase))
            .OrderBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Product> GetProductsByPriceRange(decimal minimumPrice, decimal maximumPrice)
    {
        if (minimumPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumPrice), "Minimum price cannot be negative.");
        }

        if (maximumPrice < minimumPrice)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumPrice), "Maximum price must be greater than or equal to minimum price.");
        }

        return _store.Products
            .Where(product => product.Price >= minimumPrice && product.Price <= maximumPrice)
            .OrderBy(product => product.Price)
            .ThenBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Product> GetProductsByMinimumRating(decimal minimumRating)
    {
        if (minimumRating is < 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumRating), "Minimum rating must be between 0 and 5.");
        }

        return _store.Products
            .Where(product => product.AverageRating >= minimumRating)
            .OrderByDescending(product => product.AverageRating)
            .ThenBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Product> GetInStockProducts()
    {
        return _store.Products
            .Where(product => product.StockQuantity > 0)
            .OrderBy(product => product.Category)
            .ThenBy(product => product.Name)
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
        _store.SaveProducts();
        return review;
    }
}
