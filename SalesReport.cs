namespace OnlineShoppingSystem;

/// <summary>
/// Represents aggregated sales analytics for administrators.
/// </summary>
public sealed class SalesReport
{
    public SalesReport(
        int totalOrders,
        decimal totalRevenue,
        IReadOnlyCollection<ProductSalesSummary> topProducts,
        IReadOnlyCollection<CategorySalesSummary> categorySales)
    {
        TotalOrders = totalOrders;
        TotalRevenue = totalRevenue;
        TopProducts = topProducts;
        CategorySales = categorySales;
    }

    public int TotalOrders { get; }

    public decimal TotalRevenue { get; }

    public IReadOnlyCollection<ProductSalesSummary> TopProducts { get; }

    public IReadOnlyCollection<CategorySalesSummary> CategorySales { get; }
}

/// <summary>
/// Represents product-level sales totals.
/// </summary>
public sealed class ProductSalesSummary
{
    public ProductSalesSummary(string productName, int quantitySold, decimal revenue)
    {
        ProductName = productName;
        QuantitySold = quantitySold;
        Revenue = revenue;
    }

    public string ProductName { get; }

    public int QuantitySold { get; }

    public decimal Revenue { get; }
}

/// <summary>
/// Represents category-level sales totals.
/// </summary>
public sealed class CategorySalesSummary
{
    public CategorySalesSummary(string category, int quantitySold, decimal revenue)
    {
        Category = category;
        QuantitySold = quantitySold;
        Revenue = revenue;
    }

    public string Category { get; }

    public int QuantitySold { get; }

    public decimal Revenue { get; }
}
