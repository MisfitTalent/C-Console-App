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
        PrintLowStockAlert();

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
        if (product is null)
        {
            Console.WriteLine("Add product cancelled.");
            return;
        }

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
        var products = _productService.GetProducts();
        PrintProducts(products);
        if (!TryReadProduct(products, "Product update cancelled.", out var selectedProduct) || selectedProduct is null)
        {
            return;
        }

        var product = ReadProductDetails();
        if (product is null)
        {
            Console.WriteLine("Product update cancelled.");
            return;
        }

        var updated = _productService.UpdateProduct(
            selectedProduct.Id,
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
        var products = _productService.GetProducts();
        PrintProducts(products);
        if (!TryReadProduct(products, "Product deletion cancelled.", out var product) || product is null)
        {
            return;
        }

        Console.WriteLine(_productService.DeleteProduct(product.Id) ? "Product deleted." : "Product not found.");
    }

    private void RestockProduct()
    {
        var products = _productService.GetProducts();
        PrintProducts(products);
        if (!TryReadProduct(products, "Product restock cancelled.", out var product) || product is null)
        {
            return;
        }

        if (!ConsoleInput.TryReadInt("Quantity to add:", 1, int.MaxValue, out var quantity))
        {
            Console.WriteLine("Product restock cancelled.");
            return;
        }

        Console.WriteLine(_productService.RestockProduct(product.Id, quantity) ? "Product restocked." : "Product not found.");
    }

    private void ViewProducts()
    {
        PrintProducts(_productService.GetProducts());
    }

    private void ViewOrders()
    {
        PrintOrders(_orderService.GetAllOrders());
    }

    private void UpdateOrderStatus()
    {
        var orders = _orderService.GetAllOrders();
        PrintOrders(orders);
        if (!ConsoleInput.TryReadItemById(
            "Order ID:",
            orders,
            order => order.Id,
            "order",
            out var order)
            || order is null)
        {
            Console.WriteLine("Order status update cancelled.");
            return;
        }

        if (!TryReadOrderStatus(out var status))
        {
            Console.WriteLine("Order status update cancelled.");
            return;
        }

        Console.WriteLine(_orderService.UpdateOrderStatus(order.Id, status) ? "Order status updated." : "Order not found.");
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

    private void PrintLowStockAlert()
    {
        var lowStockProducts = _productService.GetLowStockProducts();
        if (lowStockProducts.Count == 0)
        {
            return;
        }

        Console.WriteLine($"Alert: {lowStockProducts.Count} product(s) are low on stock.");
    }

    private static void PrintProducts(IReadOnlyCollection<Product> products)
    {
        ConsoleRenderer.PrintHeader("Products");
        ConsoleRenderer.PrintProducts(products);
    }

    private static void PrintOrders(IReadOnlyCollection<Order> orders)
    {
        ConsoleRenderer.PrintHeader("Orders");
        ConsoleRenderer.PrintOrders(orders);
    }

    private static bool TryReadProduct(
        IReadOnlyCollection<Product> products,
        string cancellationMessage,
        out Product? product)
    {
        if (ConsoleInput.TryReadItemByIdOrName(
            "Product ID or name:",
            products,
            selectedProduct => selectedProduct.Id,
            selectedProduct => selectedProduct.Name,
            "product",
            out product))
        {
            return true;
        }

        Console.WriteLine(cancellationMessage);
        return false;
    }

    private static ProductDetails? ReadProductDetails()
    {
        if (!ConsoleInput.TryReadRequiredText("Name:", out var name))
        {
            return null;
        }

        if (!ConsoleInput.TryReadRequiredText("Description:", out var description))
        {
            return null;
        }

        if (!ConsoleInput.TryReadRequiredText("Category:", out var category))
        {
            return null;
        }

        if (!ConsoleInput.TryReadMoney("Price:", out var price))
        {
            return null;
        }

        if (!ConsoleInput.TryReadInt("Stock quantity:", 0, int.MaxValue, out var stockQuantity))
        {
            return null;
        }

        if (!ConsoleInput.TryReadInt("Reorder level:", 0, int.MaxValue, out var reorderLevel))
        {
            return null;
        }

        return new ProductDetails(name, description, category, price, stockQuantity, reorderLevel);
    }

    private static bool TryReadOrderStatus(out OrderStatus status)
    {
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. Processing");
        Console.WriteLine("3. Shipped");
        Console.WriteLine("4. Delivered");
        Console.WriteLine("5. Cancelled");
        if (!ConsoleInput.TryReadInt("Select status:", 1, 5, out var choice))
        {
            status = OrderStatus.Pending;
            return false;
        }

        status = (OrderStatus)choice;
        return true;
    }

    private sealed record ProductDetails(
        string Name,
        string Description,
        string Category,
        decimal Price,
        int StockQuantity,
        int ReorderLevel);
}
