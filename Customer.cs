namespace OnlineShoppingSystem;

/// <summary>
/// Represents a customer who can manage a cart, fund a wallet, and place orders.
/// </summary>
public sealed class Customer : User
{
    public Customer(int id, string name, string username, string password)
        : base(id, name, username, password, UserRole.Customer)
    {
    }

    public Cart Cart { get; } = new();

    public decimal WalletBalance { get; private set; }

    /// <summary>
    /// Adds funds to the customer's wallet balance.
    /// </summary>
    public void AddWalletFunds(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        WalletBalance += amount;
    }

    /// <summary>
    /// Removes funds from the customer's wallet when enough balance is available.
    /// </summary>
    public bool TryDebitWallet(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        if (WalletBalance < amount)
        {
            return false;
        }

        WalletBalance -= amount;
        return true;
    }
}
