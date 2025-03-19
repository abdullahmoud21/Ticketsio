using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Ticketsio.Data;

namespace Ticketsio.Models
{
    public class Ticket
    {
        public  int Id { get; set; }
        public int? MovieId { get; set; }
        [ValidateNever]
        public Movie Movie { get; set; }
        public int CinemaId { get; set; }
        [ValidateNever]
        public Cinema Cinema { get; set; }
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; }
        public string SeatNumber { get; set; }
        public double Price { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public DateTime ShowTime { get; set; }
        [ValidateNever]
        public TicketStatus TicketStatus { get; set; }
        public string? TransactionId { get; set; }
    }
}
