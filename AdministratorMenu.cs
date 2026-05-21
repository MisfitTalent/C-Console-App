namespace OnlineShoppingSystem;

/// <summary>
/// Provides all administrator console operations for catalog, inventory, orders, and reports.
/// </summary>
public sealed class AdministratorMenu
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IReportService _reportService;

    public AdministratorMenu(
        IProductService productService,
        IOrderService orderService,
        IReportService reportService)
    {
        _productService = productService;
        _orderService = orderService;
        _reportService = reportService;
    }

    /// <summary>
    /// Shows the administrator menu until the user logs out.
    /// </summary>
    public void Show(Administrator administrator)
    {
        Console.WriteLine($"Administrator session started for {administrator.Name}.");

        while (true)
        {
            ConsoleRenderer.PrintHeader("Administrator Menu");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Update Product");
            Console.WriteLine("3. Delete Product");
            Console.WriteLine("4. Restock Product");
            Console.WriteLine("5. View Products");
            Console.WriteLine("6. View Orders");
            Console.WriteLine("7. Update Order Status");
            Console.WriteLine("8. View Low Stock Products");
            Console.WriteLine("9. Generate Sales Reports");
            Console.WriteLine("10. Logout");

            var choice = ConsoleInput.ReadInt("Select option: ", 1, 10);
            if (choice == 10)
            {
                return;
            }

            HandleChoice(choice);
        }
    }

    private void HandleChoice(int choice)
    {
        try
        {
            switch (choice)
            {
                case 1:
                    AddProduct();
                    break;
                case 2:
                    UpdateProduct();
                    break;
                case 3:
                    DeleteProduct();
                    break;
                case 4:
                    RestockProduct();
                    break;
                case 5:
                    ViewProducts();
                    break;
                case 6:
                    ViewOrders();
                    break;
                case 7:
                    UpdateOrderStatus();
                    break;
                case 8:
                    ViewLowStockProducts();
                    break;
                case 9:
                    GenerateSalesReport();
                    break;
            }
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            Console.WriteLine(exception.Message);
        }
        finally
        {
            ConsoleInput.WaitForMenuReturn();
        }
    }

    private void AddProduct()
    {
        var product = ReadProductDetails();
        var createdProduct = _productService.AddProduct(
            product.Name,
            product.Description,
            product.Category,
            product.Price,
            product.StockQuantity,
            product.ReorderLevel);

        Console.WriteLine($"Product {createdProduct.Id} added.");
    }

    private void UpdateProduct()
    {
        ViewProducts();
        var productId = ConsoleInput.ReadInt("Product ID: ", 1, int.MaxValue);
        var product = ReadProductDetails();

        var updated = _productService.UpdateProduct(
            productId,
            product.Name,
            product.Description,
            product.Category,
            product.Price,
            product.StockQuantity,
            product.ReorderLevel);

        Console.WriteLine(updated ? "Product updated." : "Product not found.");
    }

    private void DeleteProduct()
    {
        ViewProducts();
        var productId = ConsoleInput.ReadInt("Product ID: ", 1, int.MaxValue);
        Console.WriteLine(_productService.DeleteProduct(productId) ? "Product deleted." : "Product not found.");
    }

    private void RestockProduct()
    {
        ViewProducts();
        var productId = ConsoleInput.ReadInt("Product ID: ", 1, int.MaxValue);
        var quantity = ConsoleInput.ReadInt("Quantity to add: ", 1, int.MaxValue);
        Console.WriteLine(_productService.RestockProduct(productId, quantity) ? "Product restocked." : "Product not found.");
    }

    private void ViewProducts()
    {
        ConsoleRenderer.PrintHeader("Products");
        ConsoleRenderer.PrintProducts(_productService.GetProducts());
    }

    private void ViewOrders()
    {
        ConsoleRenderer.PrintHeader("Orders");
        ConsoleRenderer.PrintOrders(_orderService.GetAllOrders());
    }

    private void UpdateOrderStatus()
    {
        ViewOrders();
        var orderId = ConsoleInput.ReadInt("Order ID: ", 1, int.MaxValue);
        var status = ReadOrderStatus();
        Console.WriteLine(_orderService.UpdateOrderStatus(orderId, status) ? "Order status updated." : "Order not found.");
    }

    private void ViewLowStockProducts()
    {
        ConsoleRenderer.PrintHeader("Low Stock Products");
        ConsoleRenderer.PrintProducts(_productService.GetLowStockProducts());
    }

    private void GenerateSalesReport()
    {
        ConsoleRenderer.PrintHeader("Sales Report");
        ConsoleRenderer.PrintSalesReport(_reportService.GenerateSalesReport());
    }

    private static ProductDetails ReadProductDetails()
    {
        var name = ConsoleInput.ReadRequiredText("Name: ");
        var description = ConsoleInput.ReadRequiredText("Description: ");
        var category = ConsoleInput.ReadRequiredText("Category: ");
        var price = ConsoleInput.ReadMoney("Price: ");
        var stockQuantity = ConsoleInput.ReadInt("Stock quantity: ", 0, int.MaxValue);
        var reorderLevel = ConsoleInput.ReadInt("Reorder level: ", 0, int.MaxValue);

        return new ProductDetails(name, description, category, price, stockQuantity, reorderLevel);
    }

    private static OrderStatus ReadOrderStatus()
    {
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. Processing");
        Console.WriteLine("3. Shipped");
        Console.WriteLine("4. Delivered");
        Console.WriteLine("5. Cancelled");
        var choice = ConsoleInput.ReadInt("Select status: ", 1, 5);
        return (OrderStatus)choice;
    }

    private sealed record ProductDetails(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int StockQuantity,
        int ReorderLevel);
}
