namespace OnlineShoppingSystem;

/// <summary>
/// Provides validated console input helpers for menu-driven workflows.
/// </summary>
public static class ConsoleInput
{
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
}
