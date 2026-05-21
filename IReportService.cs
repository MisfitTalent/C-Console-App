namespace OnlineShoppingSystem;

/// <summary>
/// Defines reporting operations for administrative analytics.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Builds a sales report for all successful orders.
    /// </summary>
    SalesReport GenerateSalesReport();
}
