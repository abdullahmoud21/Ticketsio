using Ticketsio.DataAccess;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Repository
{
    public class TicketSeatsRepository : Repository<TicketSeats>, ITicketSeatsRepository
    {
        private readonly ApplicationDbContext dbContext;
        public TicketSeatsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
