namespace CompanionCLI.Entities;

public class Artist
{
    public string Name { get; protected set; } = string.Empty;
    public decimal PricePerHour { get; protected set; }
    public bool isFavorite { get; set; } = false;

    public Artist(string name, decimal pricePerHour)
    {
        this.Name = name;
        this.PricePerHour = pricePerHour;
    }
}


