using CompanionCLI.Entities;

namespace CompanionCLI.Repositories;

public class InMemoryConcertRepository : IConcertRepository
{
    private readonly List<Concert> concerts = [];

    public void Add(Concert concert)
    {
        concerts.Add(concert);
    }

    public IReadOnlyList<Concert> GetAll()
    {
        return concerts;
    }
}
