using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using Stripe.Checkout;
using System.Linq.Expressions;
using System.Security.Claims;
using Ticketsio.Data;
using Ticketsio.Models;
using Ticketsio.Repository;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketSeatsRepository _ticketSeatsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(IMovieRepository movieRepository, ITicketRepository ticketRepository, ISeatRepository seatRepository, UserManager<ApplicationUser> userManager, ITicketSeatsRepository ticketSeatsRepository)
        {
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _seatRepository = seatRepository;
            _userManager = userManager;
            _ticketSeatsRepository = ticketSeatsRepository;
        }
        [HttpGet]
        public IActionResult BookSeats(int MovieId)
        {
            var movie = _movieRepository.GetOne(e => e.Id == MovieId);
            var Seats = _seatRepository.Get(e => e.MovieId == MovieId, includes: new Expression<Func<Seat, object>>[] { e => e.Movie });
            ViewData["Seats"] = Seats.ToList();
            ViewData["Movie"] = movie;
            return View();
        }
        [HttpPost]
        public IActionResult BookSeats(int MovieId, string[] SelectedSeatsList)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User must be logged in.");

            var movie = _movieRepository.GetOne(e => e.Id == MovieId, includes: new Expression<Func<Movie, object>>[] { e => e.Cinema });
            if (movie == null)
                return NotFound("Movie not found.");

            var selectedSeatIds = SelectedSeatsList.First().Split(',').Select(s => s.Trim()).Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();

            var seats = _seatRepository.Get(e => selectedSeatIds.Contains(e.Id)).ToList();

            var ticket = new Ticket
            {
                MovieId = MovieId,
                CinemaId = movie.Cinema.Id,
                UserId = userId,
                BookingDate = DateTime.Now,
                ShowTime = DateTime.Now.AddHours(3),
                TicketStatus = TicketStatus.Pending,
                Price = seats.Count * movie.Price,
                TicketSeats = new List<TicketSeats>()
            };


            foreach (var seat in seats)
            {
                if (seat.IsBooked)
                    return BadRequest($"Seat {seat.SeatNumber} is already booked.");

                seat.IsBooked = true;

                var ticketSeat = new TicketSeats
                {
                    Ticket = ticket,
                    SeatId = seat.Id
                };

                ticket.TicketSeats.Add(ticketSeat);
                _seatRepository.Edit(seat);
            }

            _ticketRepository.Create(ticket);
            _ticketRepository.Commit();
            _seatRepository.Commit();

            return RedirectToAction("Pay", new { ticketId = ticket.Id });
        }



        public IActionResult Pay(int ticketid)
        {
            var ticket = _ticketSeatsRepository.Get(e => e.TicketId == ticketid, includes: new Expression<Func<TicketSeats, object>>[] { e => e.Seat, e => e.Ticket, e => e.Ticket.Movie }).ToList();
            var tickets = _ticketRepository.GetOne(e => e.Id == ticketid, includes: new Expression<Func<Ticket, object>>[] { e => e.Movie, e => e.User });


            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/Success?ticketId={tickets.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/Cancel?ticketId={tickets.Id}",
            };
            foreach (var model in ticket)
            {
                options.LineItems.Add(
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(model.Ticket.Movie.Price * 100),
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = model.Ticket.Movie.Name,
                            Description = model.Seat.SeatNumber,
                            Images = new List<string> { model.Ticket.Movie.ImgUrl },
                        },
                    },
                    Quantity = 1,
                });
            }
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);
            tickets.SessionId = session.Id;
            _ticketRepository.Edit(tickets);
            _ticketRepository.Commit();

            return Redirect(session.Url);
        }
        public IActionResult ShowTicket()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tickets = _ticketRepository.Get(e => e.UserId == userId, includes: new Expression<Func<Ticket, object>>[] { e => e.Movie, e => e.TicketSeats, e => e.Cinema});
            var ticketseats = _ticketSeatsRepository.Get(e => e.Ticket.UserId == userId, includes: new Expression<Func<TicketSeats, object>>[] { e => e.Seat, e => e.Ticket, e => e.Ticket.Movie , e => e.Ticket.Cinema }).ToList();
          ViewBag.TicketSeats = ticketseats;
            return View(tickets.ToList());
        }
    }
}
