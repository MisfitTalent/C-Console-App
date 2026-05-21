namespace OnlineShoppingSystem;

/// <summary>
/// Centralizes console rendering for products, carts, orders, and reports.
/// </summary>
public static class ConsoleRenderer
{
    /// <summary>
    /// Prints a section heading.
    /// </summary>
    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"======== {title} ========");
    }

    /// <summary>
    /// Prints products with stock and rating details.
    /// </summary>
    public static void PrintProducts(IEnumerable<Product> products)
    {
        var productList = products.ToList();
        if (productList.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in productList)
        {
            Console.WriteLine(
                $"{product.Id}. {product.Name} | {product.Category} | {product.Price:C} | Stock: {product.StockQuantity} | Rating: {product.AverageRating:0.0}");
            Console.WriteLine($"   {product.Description}");
        }
    }

    /// <summary>
    /// Prints cart contents and total.
    /// </summary>
    public static void PrintCart(Cart cart)
    {
        if (cart.IsEmpty)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        foreach (var item in cart.Items)
        {
            Console.WriteLine($"{item.Product.Id}. {item.Product.Name} x {item.Quantity} = {item.LineTotal:C}");
        }

        Console.WriteLine($"Total: {cart.Total:C}");
    }

    /// <summary>
    /// Prints order summaries with line item details.
    /// </summary>
    public static void PrintOrders(IEnumerable<Order> orders)
    {
        var orderList = orders.ToList();
        if (orderList.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        foreach (var order in orderList)
        {
            Console.WriteLine($"Order {order.Id} | {order.CreatedAt:g} | {order.Status} | Total: {order.Total:C}");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"   {item.ProductName} x {item.Quantity} = {item.LineTotal:C}");
            }
        }
    }

    /// <summary>
    /// Prints the sales report for administrators.
    /// </summary>
    public static void PrintSalesReport(SalesReport report)
    {
        Console.WriteLine($"Total orders: {report.TotalOrders}");
        Console.WriteLine($"Total revenue: {report.TotalRevenue:C}");

        Console.WriteLine();
        Console.WriteLine("Top products:");
        if (report.TopProducts.Count == 0)
        {
            Console.WriteLine("No product sales yet.");
        }

        foreach (var product in report.TopProducts)
        {
            Console.WriteLine($"{product.ProductName}: {product.QuantitySold} sold, {product.Revenue:C}");
        }

        Console.WriteLine();
        Console.WriteLine("Category sales:");
        if (report.CategorySales.Count == 0)
        {
            Console.WriteLine("No category sales yet.");
        }

        foreach (var category in report.CategorySales)
        {
            Console.WriteLine($"{category.Category}: {category.QuantitySold} sold, {category.Revenue:C}");
        }
    }
}
