using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.Get();
            if (categories != null && categories.Count() >= 1)
            {
                return View(categories.ToList());
            }
            return View("NotFound");
        }
    }
}
