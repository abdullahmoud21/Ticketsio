using Microsoft.AspNetCore.Mvc;
using Ticketsio.Repository;
using Ticketsio.Repository.IRepositories;

namespace Ticketsio.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TicketController: Controller
    {
       private readonly ITicketRepository _ticketRepository;
        private readonly IMovieRepository _movieRepository;

        public TicketController(IMovieRepository movieRepository, ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
        }
        public IActionResult BookSeats(int MovieId)
        {
            var Movie = _movieRepository.GetOne(e => e.Id == MovieId);
            ViewBag.Movie = Movie;
            return View();
        }

    }
}
