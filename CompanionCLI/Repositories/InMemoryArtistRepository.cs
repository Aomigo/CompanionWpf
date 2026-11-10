using CompanionCLI.Entities;

namespace CompanionCLI.Repositories;

internal class InMemoryArtistRepository : IArtistRepository
{
    private readonly List<Artist> artists = [
        new("Nami", 120.0m),
        new("Tame Impala", 170.0m),
        new("Grioten", 75.0m),
        new("Ado", 140.5m),
        new("Marcin", 110.3m),
        new("Aurora", 119.8m)
        ];
    public IReadOnlyList<Artist> GetAll()
    {
        return artists;
    }
}
