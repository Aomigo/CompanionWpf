using CompanionCLI.Entities;

namespace CompanionCLI.Repositories;

internal interface IArtistRepository
{

    IReadOnlyList<Artist> GetAll();
}
