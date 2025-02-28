using Ticketsio.DataAccess;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
