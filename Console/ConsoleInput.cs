namespace OnlineShoppingSystem;

/// <summary>
/// Provides validated console input helpers for menu-driven workflows.
/// </summary>
public static class ConsoleInput
{
    private const string BackPromptHint = "B to go back";

    /// <summary>
    /// Reads a required text value from the console.
    /// </summary>
    public static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Value is required.");
        }
    }

    /// <summary>
    /// Reads a required text value or returns false when the user chooses to go back.
    /// </summary>
    public static bool TryReadRequiredText(string prompt, out string value)
    {
        while (true)
        {
            Console.Write(WithBackHint(prompt));
            var input = Console.ReadLine();

            if (IsBackCommand(input))
            {
                value = string.Empty;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(input))
            {
                value = input.Trim();
                return true;
            }

            Console.WriteLine("Value is required.");
        }
    }

    /// <summary>
    /// Reads an integer value within the provided range.
    /// </summary>
    public static int ReadInt(string prompt, int minValue, int maxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (int.TryParse(input, out var value) && value >= minValue && value <= maxValue)
            {
                return value;
            }

            Console.WriteLine($"Enter a number between {minValue} and {maxValue}.");
        }
    }

    /// <summary>
    /// Reads an integer value or returns false when the user chooses to go back.
    /// </summary>
    public static bool TryReadInt(string prompt, int minValue, int maxValue, out int value)
    {
        while (true)
        {
            Console.Write(WithBackHint(prompt));
            var input = Console.ReadLine();

            if (IsBackCommand(input))
            {
                value = default;
                return false;
            }

            if (int.TryParse(input, out value) && value >= minValue && value <= maxValue)
            {
                return true;
            }

            Console.WriteLine($"Enter a number between {minValue} and {maxValue}.");
        }
    }

    /// <summary>
    /// Reads a positive decimal value from the console.
    /// </summary>
    public static decimal ReadMoney(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (decimal.TryParse(input, out var value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Enter an amount greater than zero.");
        }
    }

    /// <summary>
    /// Reads a positive decimal value or returns false when the user chooses to go back.
    /// </summary>
    public static bool TryReadMoney(string prompt, out decimal value)
    {
        while (true)
        {
            Console.Write(WithBackHint(prompt));
            var input = Console.ReadLine();

            if (IsBackCommand(input))
            {
                value = default;
                return false;
            }

            if (decimal.TryParse(input, out value) && value > 0)
            {
                return true;
            }

            Console.WriteLine("Enter an amount greater than zero.");
        }
    }

    /// <summary>
    /// Pauses the current view until the user chooses to return to the menu.
    /// </summary>
    public static void WaitForMenuReturn()
    {
        Console.WriteLine();
        Console.Write("Press Enter to return to the menu...");
        Console.ReadLine();
    }

    private static bool IsBackCommand(string? input)
    {
        return input is not null
            && (input.Equals("b", StringComparison.OrdinalIgnoreCase)
                || input.Equals("back", StringComparison.OrdinalIgnoreCase));
    }

    private static string WithBackHint(string prompt)
    {
        var cleanPrompt = prompt.Trim().TrimEnd(':');
        return $"{cleanPrompt} ({BackPromptHint}): ";
    }
}
