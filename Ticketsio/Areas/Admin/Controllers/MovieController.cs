using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository.IRepositories;
using Ticketsio.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly IActorRepository actorRepository;
        private readonly ICategoryRepository categoryRepository;
        public MovieController(IMovieRepository movieRepository, ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository , IActorRepository actorRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.actorRepository = actorRepository;
            this.cinemaRepository = cinemaRepository;
        }
        public IActionResult Index()
        {
            var movies = movieRepository.Get();
            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var cinemas = cinemaRepository.Get() ?? new List<Cinema>();
            var categories = categoryRepository.Get() ?? new List<Category>();
            var actors = actorRepository.Get() ?? new List<Actors>();

            //https://stackoverflow.com/questions/4452960/mvc-selectlist-vs-multiselectlist 

            ViewBag.Cinemas = new MultiSelectList(cinemas, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Actors = new MultiSelectList(actors, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie, List<int> ActorMovies)
        {
            if (ModelState.IsValid)
            {
                
                movie.Cinema = cinemaRepository.GetOne(e => e.Id == movie.Cinema.Id);
                movie.Category = categoryRepository.GetOne(e => e.Id == movie.Category.Id);
                movie.ActorMovies = ActorMovies.Select(actorId => new ActorMovie { ActorsId = actorId, MoviesId = movie.Id }).ToList();
                movieRepository.Create(movie);
                movieRepository.Commit();
                return RedirectToAction("Index");
            }

            // Repopulate ViewBag to prevent NullReferenceException
            ViewBag.Cinemas = new SelectList(cinemaRepository.Get(), "Id", "Name");
            ViewBag.Categories = new SelectList(categoryRepository.Get(), "Id", "Name");
            ViewBag.Actors = new MultiSelectList(actorRepository.Get(), "Id", "FullName");

            return View(movie);
        }


    }
}
