using Ticketsio.DataAccess;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Repository
{
    public class SeatRepository : Repository<Seat>, ISeatRepository
    {
        private readonly ApplicationDbContext dbContext;
        public SeatRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
