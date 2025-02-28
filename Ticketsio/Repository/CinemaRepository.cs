using Ticketsio.DataAccess;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Repository
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
