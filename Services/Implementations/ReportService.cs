namespace OnlineShoppingSystem;

/// <summary>
/// Builds administrator sales analytics from placed orders.
/// </summary>
public sealed class ReportService : IReportService
{
    private readonly AppDataStore _store;

    public ReportService(AppDataStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public SalesReport GenerateSalesReport()
    {
        var orders = _store.Orders.ToList();
        var orderItems = orders.SelectMany(order => order.Items).ToList();

        var topProducts = orderItems
            .GroupBy(item => item.ProductName)
            .Select(group => new ProductSalesSummary(
                group.Key,
                group.Sum(item => item.Quantity),
                group.Sum(item => item.LineTotal)))
            .OrderByDescending(summary => summary.Revenue)
            .ThenBy(summary => summary.ProductName)
            .Take(5)
            .ToList();

        var categorySales = orderItems
            .Join(
                _store.Products,
                item => item.ProductId,
                product => product.Id,
                (item, product) => new { product.Category, item.Quantity, item.LineTotal })
            .GroupBy(item => item.Category)
            .Select(group => new CategorySalesSummary(
                group.Key,
                group.Sum(item => item.Quantity),
                group.Sum(item => item.LineTotal)))
            .OrderByDescending(summary => summary.Revenue)
            .ToList();

        return new SalesReport(orders.Count, orders.Sum(order => order.Total), topProducts, categorySales);
    }
}
