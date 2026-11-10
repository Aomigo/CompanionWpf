using CompanionCLI.Entities;
using CompanionCLI.Repositories;

namespace CompanionCLI.Services
{
    internal class ArtistService
    {
        private readonly IArtistRepository repository;
        public ArtistService(IArtistRepository repository)
        {
            this.repository = repository;
        }
        public void MakeFavorite(string name)
        {
            foreach (Artist artist in repository.GetAll())
            {
                if (artist.Name == name && artist.isFavorite) return;

                artist.isFavorite = false;

                if (artist.Name == name) artist.isFavorite = true;
            }
        }
    }

}


