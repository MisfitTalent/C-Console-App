namespace OnlineShoppingSystem;

/// <summary>
/// Represents a product in the catalog with inventory and customer review information.
/// </summary>
public sealed class Product
{
    public Product(
        int id,
        string name,
        string description,
        string category,
        decimal price,
        int stockQuantity,
        int reorderLevel)
    {
        Id = id;
        UpdateDetails(name, description, category, price, stockQuantity, reorderLevel);
    }

    private readonly List<Review> _reviews = [];

    public int Id { get; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Category { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int StockQuantity { get; private set; }

    public int ReorderLevel { get; private set; }

    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    public decimal AverageRating => _reviews.Count == 0 ? 0 : (decimal)_reviews.Average(review => review.Rating);

    public bool IsLowStock => StockQuantity <= ReorderLevel;

    /// <summary>
    /// Updates catalog and inventory details for this product.
    /// </summary>
    public void UpdateDetails(
        string name,
        string description,
        string category,
        decimal price,
        int stockQuantity,
        int reorderLevel)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Product category is required.", nameof(category));
        }

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock cannot be negative.");
        }

        if (reorderLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(reorderLevel), "Reorder level cannot be negative.");
        }

        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        Price = price;
        StockQuantity = stockQuantity;
        ReorderLevel = reorderLevel;
    }

    /// <summary>
    /// Adds stock to the product inventory.
    /// </summary>
    public void Restock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        StockQuantity += quantity;
    }

    /// <summary>
    /// Reduces product stock when enough inventory is available.
    /// </summary>
    public bool TryReserveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (StockQuantity < quantity)
        {
            return false;
        }

        StockQuantity -= quantity;
        return true;
    }

    /// <summary>
    /// Returns reserved stock to inventory after a failed checkout.
    /// </summary>
    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        StockQuantity += quantity;
    }

    /// <summary>
    /// Adds a customer review to the product.
    /// </summary>
    public void AddReview(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);
        _reviews.Add(review);
    }
}
