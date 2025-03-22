using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using System.Linq.Expressions;
using System.Security.Claims;
using Ticketsio.Data;
using Ticketsio.Models;
using Ticketsio.Repository;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TicketController: Controller
    {
       private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(IMovieRepository movieRepository, ITicketRepository ticketRepository, ISeatRepository seatRepository, UserManager<ApplicationUser> userManager)
        {
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _seatRepository = seatRepository;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult BookSeats(int MovieId)
        {
            var movie = _movieRepository.GetOne(e => e.Id == MovieId);
            var Seats = _seatRepository.Get(e => e.MovieId == MovieId,includes: new Expression<Func<Seat, object>>[] { e => e.Movie} );
            ViewData["Seats"] = Seats.ToList();
            ViewData["Movie"] = movie;
            return View();
        }
        [HttpPost]
        public IActionResult BookSeats(int MovieId, string[] SelectedSeatsList)
        {
            var movie = _movieRepository.GetOne(e => e.Id == MovieId);
            var seats = _seatRepository.Get(e => e.MovieId == MovieId, includes: new Expression<Func<Seat, object>>[] { e => e.Movie });

            if (seats == null || movie == null)
                return NotFound("Movie or seats not found");

            // Create a new Ticket
            var ticket = new Ticket
            {
                MovieId = MovieId,
                CinemaId = movie.Cinema.Id,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ShowTime = DateTime.Now.AddHours(3),
                TicketStatus = TicketStatus.Pending,
                Price = SelectedSeatsList.Length * movie.Price,
                Seats = new List<Seat>()
            };

            foreach (var seat in seats)
            {
                if (SelectedSeatsList.Contains(seat.SeatNumber))
                {
                    seat.IsBooked = true;
                    seat.Ticket = ticket;
                    ticket.Seats.Add(seat);
                    _seatRepository.Edit(seat);
                }
            }

            _ticketRepository.Create(ticket);
            _seatRepository.Commit();
            _ticketRepository.Commit();

            return RedirectToAction("Checkout", "Payment", new { ticketId = ticket.Id });
        }

    }
}
