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
            if (!TryReadRegistrationDetails(out var name, out var username, out var password, out var role))
            {
                Console.WriteLine("Registration cancelled.");
                return;
            }

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
        if (!TryReadLoginDetails(out var username, out var password))
        {
            Console.WriteLine("Login cancelled.");
            return;
        }

        var user = _userService.Login(username, password);

        if (user is null)
        {
            Console.WriteLine("Invalid username or password.");
            return;
        }

        Console.WriteLine($"Welcome, {user.Name}.");
        RouteUser(user);
    }

    private static bool TryReadLoginDetails(out string username, out string password)
    {
        username = string.Empty;
        password = string.Empty;
        var step = 1;

        while (step is >= 1 and <= 2)
        {
            if (step == 1)
            {
                if (!ConsoleInput.TryReadRequiredText("Username:", out username))
                {
                    return false;
                }

                step++;
                continue;
            }

            if (ConsoleInput.TryReadRequiredText("Password:", out password))
            {
                step++;
                continue;
            }

            step--;
        }

        return true;
    }

    private static bool TryReadRegistrationDetails(
        out string name,
        out string username,
        out string password,
        out UserRole role)
    {
        name = string.Empty;
        username = string.Empty;
        password = string.Empty;
        role = UserRole.Customer;
        var step = 1;

        while (step is >= 1 and <= 4)
        {
            if (step == 1)
            {
                if (!ConsoleInput.TryReadRequiredText("Name:", out name))
                {
                    return false;
                }

                step++;
                continue;
            }

            if (step == 2)
            {
                if (ConsoleInput.TryReadRequiredText("Username:", out username))
                {
                    step++;
                    continue;
                }

                step--;
                continue;
            }

            if (step == 3)
            {
                if (ConsoleInput.TryReadRequiredText("Password:", out password))
                {
                    step++;
                    continue;
                }

                step--;
                continue;
            }

            if (TryReadRole(out role))
            {
                step++;
                continue;
            }

            step--;
        }

        return true;
    }

    private static bool TryReadRole(out UserRole role)
    {
        Console.WriteLine("1. Customer");
        Console.WriteLine("2. Administrator");
        if (!ConsoleInput.TryReadInt("Select role:", 1, 2, out var roleChoice))
        {
            role = UserRole.Customer;
            return false;
        }

        role = roleChoice == 2 ? UserRole.Administrator : UserRole.Customer;
        return true;
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
