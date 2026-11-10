using CompanionCLI.Entities;
using CompanionCLI.Repositories;

namespace CompanionCLI.Services
{
    internal class ConcertService
    {
        private readonly IConcertRepository repository;
        public ConcertService(IConcertRepository repository)
        {
            this.repository = repository;
        }


        public void AddConcert(Artist artist, DateTime concertStart, DateTime concertEnd, decimal concertPrice, int scene, DateTime festivalStart, DateTime festivalEnd)
        {
            repository.Add(new Concert()
            {
                Artist = artist,
                ConcertStart = concertStart,
                ConcertEnd = concertEnd,
                ConcertPrice = concertPrice,
                Scene = scene
            });
        }

        public bool CanScheduleConcert(DateTime start, DateTime end, int scene, DateTime festivalStart, DateTime festivalEnd)
        {
            if (end <= start)
            {
                return false;
            }
             if(start < DateTime.Now || end < DateTime.Now)
                {
                    return false;
                }
                if(start > festivalEnd || start < festivalStart || end > festivalEnd || end < festivalStart)
                {
                    return false;
                }

            foreach (var concert in repository.GetAll())
            {
                if (concert.Scene != scene)
                {
                    continue;
                }

                bool overlaps = start < concert.ConcertEnd && end > concert.ConcertStart;
                if (overlaps)
                {
                    return false;
                }
            }
            return true;
        } //CHECK OUT OF FESTIVAL TIME BOUND

        public bool FestivalEnded(DateTime end)
        {
            return end <= DateTime.Now;
        }

        public IEnumerable<IGrouping<int, Concert>> GetSchedule(DateOnly day)
        {

            return repository.GetAll()
                .Where(x => DateOnly.FromDateTime(x.ConcertStart) == day)
                .OrderBy(x => x.Scene)
                .ThenBy(x => x.ConcertStart)
                .GroupBy(x => x.Scene);
        }
    }
}