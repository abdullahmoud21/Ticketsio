using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository.IRepositories;
using Ticketsio.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Ticketsio.Repository;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly IActorRepository actorRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISeatRepository _seatRepository;
        public MovieController(ISeatRepository seatRepository,IMovieRepository movieRepository, ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository , IActorRepository actorRepository)
        {
            this.movieRepository = movieRepository;
            this.categoryRepository = categoryRepository;
            this.actorRepository = actorRepository;
            this.cinemaRepository = cinemaRepository;
            _seatRepository = seatRepository;
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

            ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
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
                movie.MovieStatus = default;
                movieRepository.Create(movie);
                movieRepository.Commit();
                GenerateSeatsForMovie(movie.Id, movie.Cinema.Id);
                return RedirectToAction("Index");
            }

            ViewBag.Cinemas = new SelectList(cinemaRepository.Get(), "Id", "Name");
            ViewBag.Categories = new SelectList(categoryRepository.Get(), "Id", "Name");
            ViewBag.Actors = new MultiSelectList(actorRepository.Get(), "Id", "FullName");

            return View(movie);
        }
        public IActionResult Delete(int id)
        {
            var movie = movieRepository.GetOne(e => e.Id == id);
            if(movie != null)
            {
                movieRepository.Delete(movie);
                movieRepository.Commit();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = movieRepository.GetOne(e => e.Id==id, new Expression<Func<Movie, object>>[] { e => e.Category, e => e.Cinema, e => e.ActorMovies });
            
            var cinemas = cinemaRepository.Get() ?? new List<Cinema>();
            var categories = categoryRepository.Get() ?? new List<Category>();
            var actors = actorRepository.Get() ?? new List<Actors>();
            var selectedActorIds = movie.ActorMovies?.Select(am => am.ActorsId).ToList() ?? new List<int>();
            ViewBag.Cinemas = new SelectList(cinemas, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Actors = new MultiSelectList(actors, "Id", "FullName", selectedActorIds);
            return View(movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, List<int> ActorMovies)
        {
            var existingMovie = movieRepository.GetOne(e => e.Id == movie.Id,new Expression<Func<Movie, object>>[] { e => e.Category, e => e.Cinema, e => e.ActorMovies });
            if (ModelState.IsValid)
            {
                existingMovie.ImgUrl = movie.ImgUrl;
                existingMovie.Name = movie.Name;
                existingMovie.Description = movie.Description;
                existingMovie.Price = movie.Price;
                existingMovie.StartDate = movie.StartDate;
                existingMovie.EndDate = movie.EndDate;
                existingMovie.Cinema.Id = movie.Cinema.Id;
                existingMovie.Category.Id = movie.Category.Id;
                existingMovie.ActorMovies = ActorMovies.Select(actorId => new ActorMovie
                {
                    MoviesId = movie.Id,
                    ActorsId = actorId
                }).ToList();
                movieRepository.Edit(existingMovie);
                movieRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(movie);
        }
        private void GenerateSeatsForMovie(int movieId, int cinemaId)
        {
            List<Seat> seats = new List<Seat>();
            string[] rows = { "A", "B", "C", "D", "E" };
            int seatsPerRow = 9;

            foreach (var row in rows)
            {
                for (int i = 1; i <= seatsPerRow; i++)
                {
                    seats.Add(new Seat
                    {
                        MovieId = movieId,
                        SeatNumber = $"{row}{i}",
                        IsBooked = false
                    });
                }
            }

            _seatRepository.Create(seats);
            _seatRepository.Commit();
        }
    }
}
