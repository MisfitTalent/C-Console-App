namespace OnlineShoppingSystem;

/// <summary>
/// Provides all customer console operations for shopping and order tracking.
/// </summary>
public sealed class CustomerMenu
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

    public CustomerMenu(IProductService productService, IOrderService orderService, IUserService userService)
    {
        _productService = productService;
        _orderService = orderService;
        _userService = userService;
    }

    /// <summary>
    /// Shows the customer menu until the user logs out.
    /// </summary>
    public void Show(Customer customer)
    {
        while (true)
        {
            ConsoleRenderer.PrintHeader("Customer Menu");
            Console.WriteLine("1. Browse Products");
            Console.WriteLine("2. Search Products");
            Console.WriteLine("3. Add Product to Cart");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Update Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. View Wallet Balance");
            Console.WriteLine("8. Add Wallet Funds");
            Console.WriteLine("9. View Order History");
            Console.WriteLine("10. Track Orders");
            Console.WriteLine("11. Review Products");
            Console.WriteLine("12. View Payment History");
            Console.WriteLine("13. Logout");

            var choice = ConsoleInput.ReadInt("Select option: ", 1, 13);
            if (choice == 13)
            {
                return;
            }

            HandleChoice(customer, choice);
        }
    }

    private void HandleChoice(Customer customer, int choice)
    {
        try
        {
            switch (choice)
            {
                case 1:
                    BrowseProducts();
                    break;
                case 2:
                    SearchProducts();
                    break;
                case 3:
                    AddProductToCart(customer);
                    break;
                case 4:
                    ViewCart(customer);
                    break;
                case 5:
                    UpdateCart(customer);
                    break;
                case 6:
                    Checkout(customer);
                    break;
                case 7:
                    ViewWalletBalance(customer);
                    break;
                case 8:
                    AddWalletFunds(customer);
                    break;
                case 9:
                    ViewOrderHistory(customer);
                    break;
                case 10:
                    TrackOrders(customer);
                    break;
                case 11:
                    ReviewProduct(customer);
                    break;
                case 12:
                    ViewPaymentHistory(customer);
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

    private void BrowseProducts()
    {
        ConsoleRenderer.PrintHeader("Products");
        ConsoleRenderer.PrintProducts(_productService.GetProducts());
    }

    private void SearchProducts()
    {
        Console.WriteLine("1. Search by keyword");
        Console.WriteLine("2. Filter by category");
        Console.WriteLine("3. Filter by price range");
        Console.WriteLine("4. Filter by minimum rating");
        Console.WriteLine("5. Show in-stock products");

        if (!ConsoleInput.TryReadInt("Select search option:", 1, 5, out var choice))
        {
            Console.WriteLine("Search cancelled.");
            return;
        }

        var products = choice switch
        {
            1 => SearchByKeyword(),
            2 => FilterByCategory(),
            3 => FilterByPriceRange(),
            4 => FilterByMinimumRating(),
            5 => _productService.GetInStockProducts(),
            _ => []
        };

        ConsoleRenderer.PrintProducts(products);
    }

    private void AddProductToCart(Customer customer)
    {
        var products = _productService.GetProducts();
        ConsoleRenderer.PrintProducts(products);
        if (!TryReadProduct(products, "Add to cart cancelled.", out var product) || product is null)
        {
            return;
        }

        if (product.StockQuantity == 0)
        {
            Console.WriteLine("Product is out of stock.");
            return;
        }

        if (!ConsoleInput.TryReadInt("Quantity:", 1, product.StockQuantity, out var quantity))
        {
            Console.WriteLine("Add to cart cancelled.");
            return;
        }

        customer.Cart.AddProduct(product, quantity);
        _userService.SaveUsers();
        Console.WriteLine("Product added to cart.");
    }

    private static void ViewCart(Customer customer)
    {
        ConsoleRenderer.PrintHeader("Cart");
        ConsoleRenderer.PrintCart(customer.Cart);
    }

    private void UpdateCart(Customer customer)
    {
        ConsoleRenderer.PrintCart(customer.Cart);
        if (customer.Cart.IsEmpty)
        {
            return;
        }

        if (!ConsoleInput.TryReadItemByIdOrName(
            "Product ID or name:",
            customer.Cart.Items,
            item => item.Product.Id,
            item => item.Product.Name,
            "cart product",
            out var cartItem)
            || cartItem is null)
        {
            Console.WriteLine("Cart update cancelled.");
            return;
        }

        var productId = cartItem.Product.Id;
        var maximumQuantity = Math.Max(cartItem.Product.StockQuantity, cartItem.Quantity);
        Console.WriteLine("1. Change quantity");
        Console.WriteLine("2. Remove product");
        if (!ConsoleInput.TryReadInt("Select option:", 1, 2, out var choice))
        {
            Console.WriteLine("Cart update cancelled.");
            return;
        }

        if (choice == 2)
        {
            if (customer.Cart.RemoveProduct(productId))
            {
                _userService.SaveUsers();
                Console.WriteLine("Product removed.");
            }
            else
            {
                Console.WriteLine("Product not found in cart.");
            }

            return;
        }

        if (!ConsoleInput.TryReadInt("New quantity:", 1, maximumQuantity, out var quantity))
        {
            Console.WriteLine("Cart update cancelled.");
            return;
        }

        if (customer.Cart.UpdateProductQuantity(productId, quantity))
        {
            _userService.SaveUsers();
            Console.WriteLine("Cart updated.");
            return;
        }

        Console.WriteLine("Product not found in cart.");
    }

    private void Checkout(Customer customer)
    {
        ConsoleRenderer.PrintCart(customer.Cart);
        if (customer.Cart.IsEmpty)
        {
            return;
        }

        var order = _orderService.PlaceOrder(customer);
        Console.WriteLine($"Order {order.Id} placed successfully. Total: {order.Total:C}");
        ConsoleRenderer.PrintReceipt(customer, order, _orderService.GetPaymentForOrder(order.Id));
    }

    private static void ViewWalletBalance(Customer customer)
    {
        Console.WriteLine($"Wallet balance: {customer.WalletBalance:C}");
    }

    private void AddWalletFunds(Customer customer)
    {
        if (!ConsoleInput.TryReadMoney("Amount:", out var amount))
        {
            Console.WriteLine("Wallet funding cancelled.");
            return;
        }

        customer.AddWalletFunds(amount);
        _userService.SaveUsers();
        Console.WriteLine($"Wallet balance: {customer.WalletBalance:C}");
    }

    private void ViewOrderHistory(Customer customer)
    {
        ConsoleRenderer.PrintOrders(_orderService.GetOrdersForCustomer(customer.Id));
    }

    private void TrackOrders(Customer customer)
    {
        var orders = _orderService.GetOrdersForCustomer(customer.Id);
        ConsoleRenderer.PrintOrders(orders);
    }

    private void ViewPaymentHistory(Customer customer)
    {
        ConsoleRenderer.PrintHeader("Payment History");
        ConsoleRenderer.PrintPayments(_orderService.GetPaymentsForCustomer(customer.Id));
    }

    private void ReviewProduct(Customer customer)
    {
        var products = _productService.GetProducts();
        ConsoleRenderer.PrintProducts(products);
        if (!TryReadProduct(products, "Review cancelled.", out var product) || product is null)
        {
            return;
        }

        if (!ConsoleInput.TryReadInt("Rating (1-5):", 1, 5, out var rating))
        {
            Console.WriteLine("Review cancelled.");
            return;
        }

        if (!ConsoleInput.TryReadRequiredText("Comment:", out var comment))
        {
            Console.WriteLine("Review cancelled.");
            return;
        }

        _productService.AddReview(customer, product.Id, rating, comment);
        Console.WriteLine("Review added.");
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

    private IReadOnlyCollection<Product> SearchByKeyword()
    {
        if (!ConsoleInput.TryReadRequiredText("Search:", out var searchTerm))
        {
            Console.WriteLine("Search cancelled.");
            return [];
        }

        return _productService.SearchProducts(searchTerm);
    }

    private IReadOnlyCollection<Product> FilterByCategory()
    {
        if (!ConsoleInput.TryReadRequiredText("Category:", out var category))
        {
            Console.WriteLine("Search cancelled.");
            return [];
        }

        return _productService.GetProductsByCategory(category);
    }

    private IReadOnlyCollection<Product> FilterByPriceRange()
    {
        if (!ConsoleInput.TryReadMoney("Minimum price:", out var minimumPrice))
        {
            Console.WriteLine("Search cancelled.");
            return [];
        }

        if (!ConsoleInput.TryReadMoney("Maximum price:", out var maximumPrice))
        {
            Console.WriteLine("Search cancelled.");
            return [];
        }

        return _productService.GetProductsByPriceRange(minimumPrice, maximumPrice);
    }

    private IReadOnlyCollection<Product> FilterByMinimumRating()
    {
        if (!ConsoleInput.TryReadDecimal("Minimum rating (0-5):", 0, 5, out var minimumRating))
        {
            Console.WriteLine("Search cancelled.");
            return [];
        }

        return _productService.GetProductsByMinimumRating(minimumRating);
    }
}
