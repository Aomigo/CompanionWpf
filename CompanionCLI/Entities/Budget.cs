namespace CompanionCLI.Entities;

public class Budget
{
    public decimal Fund { get; private set; }

    public Budget(decimal fund)
    {
        this.Fund = fund;
    }

    public void AddFunds(decimal amount)
    {
        Fund += amount;
    }

    public bool RemoveFunds(decimal amount)
    {
        if (Fund < amount)
        {
            return false;
        }
        Fund -= amount;
        return true;
    }

}