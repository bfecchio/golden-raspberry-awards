using Awards.Application.Core.Abstractions.Data;
using Awards.Domain.Entities;
using Awards.Domain.Repositories;

namespace Awards.Persistence.Repositories
{
    internal sealed class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(IDbContext dbContext)
            : base(dbContext)
        { }
    }
}
