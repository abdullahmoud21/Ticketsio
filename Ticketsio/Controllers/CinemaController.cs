using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Controllers
{
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
            return View(Cinemas.ToList());
        }
    }
}
