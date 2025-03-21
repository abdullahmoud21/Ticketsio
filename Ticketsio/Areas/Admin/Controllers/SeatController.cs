using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ticketsio.Models;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SeatController : Controller
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMovieRepository _movieRepository;

        public SeatController(ISeatRepository seatRepository, IMovieRepository movieRepository)
        {
            _seatRepository = seatRepository;
            _movieRepository = movieRepository;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index(int? movieId)
        {
            var seats = _seatRepository.Get(includes: new Expression<Func<Seat, object>>[] { e => e.Movie });
            ViewBag.Movies = _movieRepository.Get().ToList();
            ViewBag.SelectedMovieId = movieId;
            if(movieId.HasValue)
            {
                seats = seats.Where(e => e.MovieId == movieId);
            }
            return View(seats.ToList());
        }
        public IActionResult Delete(int id) {
            var seat = _seatRepository.GetOne(e => e.Id == id);
            if(seat != null)
            {
                _seatRepository.Delete(seat);
                _seatRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(NotFound));
        }
        [HttpPost]
        public IActionResult DeleteSelectedSeats(List<int> selectedSeats)
        {
            if (selectedSeats == null || !selectedSeats.Any())
            {
                ModelState.AddModelError("", "No seats selected.");
                return RedirectToAction(nameof(Index));
            }

            var seatsToDelete = _seatRepository.Get(e => selectedSeats.Contains(e.Id)).ToList();

            if (seatsToDelete.Any())
            {
                foreach (var seat in seatsToDelete)
                {
                    _seatRepository.Delete(seat);
                }
                _seatRepository.Commit();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Create()
        {
            var movies = _movieRepository.Get();
            ViewBag.Movies = new SelectList(movies, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int movieId, int Rows, int SeatsPerRow)
        {
            var seats = _seatRepository.Get(e => e.MovieId == movieId);
            if (seats.Any() || movieId == 0 || Rows == 0 || SeatsPerRow == 0)
            {
                ModelState.AddModelError("", "Invalid input values or seats already exists");
                ViewBag.Movies = new SelectList(_movieRepository.Get(), "Id", "Name");
                return View();
            }
            GenerateSeatsForMovie(movieId, Rows, SeatsPerRow);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Edit(Seat seat)
        {
            if (seat == null || seat.Id == 0)
            {
                Console.WriteLine("Received empty or invalid seat data.");
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine($"Edit action hit - Seat ID: {seat.Id}, Number: {seat.SeatNumber}, Movie ID: {seat.MovieId}, IsBooked: {seat.IsBooked}");

            var existingSeat = _seatRepository.Get(s => s.Id == seat.Id).FirstOrDefault();
            if (existingSeat != null)
            {
                existingSeat.SeatNumber = seat.SeatNumber;
                existingSeat.MovieId = seat.MovieId;
                existingSeat.IsBooked = seat.IsBooked;
                _seatRepository.Commit();

                Console.WriteLine($"Seat {seat.Id} updated successfully.");
            }
            else
            {
                Console.WriteLine($"Seat with ID {seat.Id} not found.");
            }

            return RedirectToAction(nameof(Index));
        }



        private void GenerateSeatsForMovie(int movieId, int rows, int seatsPerRow)
        {
            List<Seat> seats = new List<Seat>();
            char rowLetter = 'A';

            for (int r = 0; r < rows; r++)
            {
                for (int s = 1; s <= seatsPerRow; s++)
                {
                    seats.Add(new Seat
                    {
                        MovieId = movieId,
                        SeatNumber = $"{rowLetter}{s}",
                        IsBooked = false
                    });
                }
                rowLetter++;
            }

            _seatRepository.Create(seats);
            _seatRepository.Commit();
        }

    }
}
