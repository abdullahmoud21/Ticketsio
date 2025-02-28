using Ticketsio.DataAccess;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Repository
{
    public class ActorRepository : Repository<Actors>, IActorRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
