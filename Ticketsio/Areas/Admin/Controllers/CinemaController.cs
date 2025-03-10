using Microsoft.AspNetCore.Mvc;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;
        private readonly IMovieRepository movieRepository;
        public CinemaController(ICinemaRepository cinemaRepository, IMovieRepository movieRepository)
        {
            this.cinemaRepository = cinemaRepository;
            this.movieRepository = movieRepository;
        }
        public IActionResult Index()
        {
            var cinemas = cinemaRepository.Get();
            return View(cinemas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema)
        {
            if (ModelState.IsValid) { 
            cinemaRepository.Create(cinema);
                cinemaRepository.Commit();
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddSeconds(10),
                    Secure = true
                };
                Response.Cookies.Append("notification", "Created Cinema Successfuly", cookieOptions);
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        public IActionResult Delete(int id)
        {
            var cinema = cinemaRepository.GetOne(e  => e.Id == id);
            var movieswithcinema = movieRepository.Get(e => e.Cinema.Id == id).ToList();
            if (cinema != null)
            {
                if (movieswithcinema.Any())
                { 
                movieRepository.Delete(movieswithcinema);
                }
                cinemaRepository.Delete(cinema);
                cinemaRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cinema = cinemaRepository.GetOne(e =>e.Id == id);
            return View(cinema);
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {
            if (ModelState.IsValid) {
                cinemaRepository.Edit(cinema);
                cinemaRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
    }
}
