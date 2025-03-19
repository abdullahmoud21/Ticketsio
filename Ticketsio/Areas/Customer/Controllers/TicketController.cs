using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
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

        public TicketController(IMovieRepository movieRepository, ITicketRepository ticketRepository, ISeatRepository seatRepository)
        {
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
            _seatRepository = seatRepository;
        }
        [HttpGet]
        public IActionResult BookSeats(int MovieId)
        {
            var Movie = _movieRepository.GetOne(e => e.Id == MovieId);
            if(Movie == null)
            {
                return NotFound();
            }
            var Seats = _seatRepository.Get(e => e.MovieId == MovieId);
            ViewBag.Movie = Movie;
            ViewBag.Seats = Seats.ToList();
            return View();
        }

    }
}
