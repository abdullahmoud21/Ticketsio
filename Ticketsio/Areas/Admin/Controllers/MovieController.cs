using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }
        public IActionResult Index()
        {
            var movies = movieRepository.Get();
            return View(movies);
        }
    }
}
