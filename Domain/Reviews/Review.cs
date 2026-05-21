namespace OnlineShoppingSystem;

/// <summary>
/// Represents a customer's rating and comment for a product.
/// </summary>
public sealed class Review
{
    public Review(int id, int customerId, int productId, int rating, string comment)
    {
        if (rating is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
        }

        Id = id;
        CustomerId = customerId;
        ProductId = productId;
        Rating = rating;
        Comment = comment.Trim();
        CreatedAt = DateTime.Now;
    }

    public int Id { get; }

    public int CustomerId { get; }

    public int ProductId { get; }

    public int Rating { get; }

    public string Comment { get; }

    public DateTime CreatedAt { get; }
}
