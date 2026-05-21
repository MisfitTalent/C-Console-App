namespace OnlineShoppingSystem;

/// <summary>
/// Handles order placement, stock reservation, payment, and order status tracking.
/// </summary>
public sealed class OrderService : IOrderService
{
    private readonly AppDataStore _store;
    private readonly IPaymentProcessor _paymentProcessor;

    public OrderService(AppDataStore store, IPaymentProcessor paymentProcessor)
    {
        _store = store;
        _paymentProcessor = paymentProcessor;
    }

    /// <inheritdoc />
    public Order PlaceOrder(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        if (customer.Cart.IsEmpty)
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        ValidateCartStock(customer.Cart);
        ReserveCartStock(customer.Cart);

        var orderItems = customer.Cart.Items
            .Select(item => new OrderItem(item.Product.Id, item.Product.Name, item.Product.Price, item.Quantity))
            .ToList();

        var order = new Order(_store.GetNextOrderId(), customer.Id, orderItems);
        var payment = _paymentProcessor.ProcessPayment(customer, order);

        if (payment.Status == PaymentStatus.Failed)
        {
            ReleaseCartStock(customer.Cart);
            throw new InvalidOperationException(payment.Message);
        }

        _store.Orders.Add(order);
        customer.Cart.Clear();
        _store.SaveUsers();
        _store.SaveProducts();
        _store.SaveOrders();
        _store.SavePayments();
        return order;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Order> GetOrdersForCustomer(int customerId)
    {
        return _store.Orders
            .Where(order => order.CustomerId == customerId)
            .OrderByDescending(order => order.CreatedAt)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Order> GetAllOrders()
    {
        return _store.Orders
            .OrderByDescending(order => order.CreatedAt)
            .ToList();
    }

    /// <inheritdoc />
    public bool UpdateOrderStatus(int orderId, OrderStatus status)
    {
        var order = _store.Orders.FirstOrDefault(order => order.Id == orderId);
        if (order is null)
        {
            return false;
        }

        order.UpdateStatus(status);
        _store.SaveOrders();
        return true;
    }

    private static void ValidateCartStock(Cart cart)
    {
        var unavailableItem = cart.Items.FirstOrDefault(item => item.Quantity > item.Product.StockQuantity);
        if (unavailableItem is not null)
        {
            throw new InvalidOperationException($"Insufficient stock for {unavailableItem.Product.Name}.");
        }
    }

    private static void ReserveCartStock(Cart cart)
    {
        foreach (var item in cart.Items)
        {
            item.Product.TryReserveStock(item.Quantity);
        }
    }

    private static void ReleaseCartStock(Cart cart)
    {
        foreach (var item in cart.Items)
        {
            item.Product.ReleaseStock(item.Quantity);
        }
    }
}
