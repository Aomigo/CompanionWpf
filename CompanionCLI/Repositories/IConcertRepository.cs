using CompanionCLI.Entities;

namespace CompanionCLI.Repositories;

internal interface IConcertRepository
{
    public void Add(Concert concert);

    IReadOnlyList<Concert> GetAll();
}
