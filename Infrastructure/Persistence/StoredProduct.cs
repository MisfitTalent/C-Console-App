namespace OnlineShoppingSystem;

/// <summary>
/// Represents the JSON-friendly shape used to persist products and reviews.
/// </summary>
public sealed class StoredProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public int ReorderLevel { get; set; }

    public List<StoredReview> Reviews { get; set; } = [];

    /// <summary>
    /// Creates a stored product from a runtime product.
    /// </summary>
    public static StoredProduct FromProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return new StoredProduct
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ReorderLevel = product.ReorderLevel,
            Reviews = product.Reviews.Select(StoredReview.FromReview).ToList()
        };
    }

    /// <summary>
    /// Creates a runtime product from this stored product.
    /// </summary>
    public Product ToProduct()
    {
        var product = new Product(Id, Name, Description, Category, Price, StockQuantity, ReorderLevel);
        foreach (var review in Reviews)
        {
            product.AddReview(review.ToReview());
        }

        return product;
    }
}

/// <summary>
/// Represents a JSON-friendly product review linked to customer and product identifiers.
/// </summary>
public sealed class StoredReview
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Creates a stored review from a runtime review.
    /// </summary>
    public static StoredReview FromReview(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        return new StoredReview
        {
            Id = review.Id,
            CustomerId = review.CustomerId,
            ProductId = review.ProductId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }

    /// <summary>
    /// Creates a runtime review from this stored review.
    /// </summary>
    public Review ToReview()
    {
        return new Review(Id, CustomerId, ProductId, Rating, Comment, CreatedAt);
    }
}
