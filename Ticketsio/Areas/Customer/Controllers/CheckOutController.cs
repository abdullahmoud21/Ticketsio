using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticketsio.Models;
using Ticketsio.Data;
using Ticketsio.Repository.IRepositories;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using System.Linq.Expressions;

namespace Ticketsio.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckOutController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketSeatsRepository _ticketSeatsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckOutController(ITicketRepository ticketRepository, IMovieRepository movieRepository, ISeatRepository seatRepository, ITicketSeatsRepository ticketSeatsRepository, UserManager<ApplicationUser> userManager)
        {
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _seatRepository = seatRepository;
            _ticketSeatsRepository = ticketSeatsRepository;
            _userManager = userManager;
        }

        public IActionResult Success(int ticketId)
        {


            var ticket = _ticketRepository.GetOne(e => e.Id == ticketId, includes: new Expression<Func<Ticket, object>>[]
            {
        e => e.Movie,
        e => e.User
            });
            var ticketseat = _ticketSeatsRepository.Get(e => e.TicketId == ticketId, includes: new Expression<Func<TicketSeats, object>>[] { e => e.Seat });
            {

                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }

                try
                {
                    var service = new SessionService();
                    Session session = service.Get(ticket.SessionId);

                    if (session == null || string.IsNullOrEmpty(session.PaymentIntentId))
                    {
                        return BadRequest("Failed to retrieve session details.");
                    }
                    foreach (var seat in ticketseat)
                    {
                        seat.Seat.IsConfirmed = true;
                        _seatRepository.Edit(seat.Seat);
                    }
                    ticket.PaymentStripeId = session.PaymentIntentId;
                    ticket.TicketStatus = TicketStatus.Paid;
                    _ticketRepository.Edit(ticket);
                    _ticketRepository.Commit();
                    _ticketSeatsRepository.Commit();
                    return View(ticket);
                }
                catch (StripeException ex)
                {
                    return StatusCode(500, "Stripe error: " + ex.Message);
                }
            }
        }

        public IActionResult Cancel(int ticketid)
        {
            var ticket = _ticketRepository.GetOne(e => e.Id == ticketid, includes: new Expression<Func<Ticket, object>>[] { e => e.Movie, e => e.User });
            var seatticket = _ticketSeatsRepository.Get(e => e.TicketId == ticketid, includes: new Expression<Func<TicketSeats, object>>[] { e => e.Seat });
            if (ticket != null)
            {
                ticket.TicketStatus = TicketStatus.Canceled;
                foreach (var seat in seatticket)
                {
                    seat.Seat.IsBooked = false;
                    _ticketSeatsRepository.Delete(seat);
                    _seatRepository.Edit(seat.Seat);
                }
                _seatRepository.Commit();
                _ticketRepository.Edit(ticket);
                _ticketRepository.Commit();
            }
            return View();
        }
    }
}
