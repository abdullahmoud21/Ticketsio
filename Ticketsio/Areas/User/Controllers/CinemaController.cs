using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.User.Controllers
{
    [Area("User")]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;
        public CinemaController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }
        public IActionResult Index()
        {
            var Cinemas = cinemaRepository.Get();
            if (Cinemas != null && Cinemas.Count() >= 1)
            {
                return View(Cinemas.ToList());
            }
            return View("NotFound");
        }
    }
}
