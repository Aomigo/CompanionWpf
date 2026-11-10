namespace CompanionCLI.Entities;

public class Concert
{
    public required Artist Artist { get; set; }
    public DateTime ConcertStart { get; set; }
    public DateTime ConcertEnd { get; set; }
    public decimal ConcertPrice { get; set; }
    public int Scene { get; set; }

}

