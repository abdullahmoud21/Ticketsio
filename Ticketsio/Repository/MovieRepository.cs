using Ticketsio.DataAccess;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Repository
{
    public class MovieRepository : Repository<Movie> , IMovieRepository
    {
        private readonly ApplicationDbContext dbContext;
        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
