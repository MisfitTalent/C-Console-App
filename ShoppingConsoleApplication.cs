namespace OnlineShoppingSystem;

/// <summary>
/// Coordinates the main console workflow and role-based menu routing.
/// </summary>
public sealed class ShoppingConsoleApplication
{
    private readonly IUserService _userService;
    private readonly CustomerMenu _customerMenu;
    private readonly AdministratorMenu _administratorMenu;

    public ShoppingConsoleApplication()
    {
        var store = new AppDataStore();
        var productService = new ProductService(store);
        var paymentProcessor = new WalletPaymentProcessor(store);
        var orderService = new OrderService(store, paymentProcessor);
        var reportService = new ReportService(store);

        _userService = new UserService(store);
        _customerMenu = new CustomerMenu(productService, orderService);
        _administratorMenu = new AdministratorMenu(productService, orderService, reportService);
    }

    /// <summary>
    /// Starts the menu-driven console application.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            ConsoleRenderer.PrintHeader("Online Shopping Backend System");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            var choice = ConsoleInput.ReadInt("Select option: ", 1, 3);
            if (choice == 1)
            {
                Register();
                continue;
            }

            if (choice == 2)
            {
                Login();
                continue;
            }

            Console.WriteLine("Goodbye.");
            return;
        }
    }

    private void Register()
    {
        try
        {
            ConsoleRenderer.PrintHeader("Register");
            var name = ConsoleInput.ReadRequiredText("Name: ");
            var username = ConsoleInput.ReadRequiredText("Username: ");
            var password = ConsoleInput.ReadRequiredText("Password: ");
            var role = ReadRole();

            var user = _userService.Register(name, username, password, role);
            Console.WriteLine($"Registered {user.Name} as {user.Role}.");
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            Console.WriteLine(exception.Message);
        }
    }

    private void Login()
    {
        ConsoleRenderer.PrintHeader("Login");
        var username = ConsoleInput.ReadRequiredText("Username: ");
        var password = ConsoleInput.ReadRequiredText("Password: ");
        var user = _userService.Login(username, password);

        if (user is null)
        {
            Console.WriteLine("Invalid username or password.");
            return;
        }

        Console.WriteLine($"Welcome, {user.Name}.");
        RouteUser(user);
    }

    private static UserRole ReadRole()
    {
        Console.WriteLine("1. Customer");
        Console.WriteLine("2. Administrator");
        var roleChoice = ConsoleInput.ReadInt("Select role: ", 1, 2);
        return roleChoice == 2 ? UserRole.Administrator : UserRole.Customer;
    }

    private void RouteUser(User user)
    {
        if (user is Customer customer)
        {
            _customerMenu.Show(customer);
            return;
        }

        if (user is Administrator administrator)
        {
            _administratorMenu.Show(administrator);
        }
    }
}
