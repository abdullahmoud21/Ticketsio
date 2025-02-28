using Microsoft.AspNetCore.Mvc;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;
using System.Linq.Expressions;

namespace Ticketsio.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        public MovieController(IMovieRepository movieRepository, IActorRepository actorRepository)
        {
            this.movieRepository = movieRepository;
            this.actorRepository = actorRepository;
        }
        public IActionResult ViewMovies()
        {
            var movies = movieRepository.Get(includes: new Expression<Func<Movie, object>>[] { e => e.Category, e => e.Cinema });
            ViewData["Movies"] = movies.ToList();
            return View();
        }
        public IActionResult Details(int movieId)
        {
            var movie = movieRepository.GetOne( e => e.Id == movieId, new Expression<Func<Movie, object>>[] { e => e.Category, e => e.Cinema });
            var actors = actorRepository.Get(
        e => e.ActorMovies.Any(am => am.MoviesId == movieId),
        new Expression<Func<Actors, object>>[]
        {
        e => e.ActorMovies
        }
    );



            ViewBag.Actors = actors.ToList();
                return View(movie);
        }
        public IActionResult ViewByCategory(int categoryId)
        {
            var movies = movieRepository.Get(e => e.Category.Id == categoryId , new Expression<Func<Movie, object>>[] {e => e.Category , e => e.Cinema});
            return View(movies.ToList());
        }
        public IActionResult ViewByCinema(int cinemaId)
        {
            var movies = movieRepository.Get(e => e.Cinema.Id == cinemaId, new Expression<Func<Movie, object>>[] { e => e.Category, e => e.Cinema });
            return View(movies.ToList());
        }
    }
}
