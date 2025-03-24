using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Ticketsio.Data;
using Ticketsio.Models;

public class Ticket
{
    public int Id { get; set; }
    public int? MovieId { get; set; }
    [ValidateNever]
    public Movie Movie { get; set; }

    public int CinemaId { get; set; }
    [ValidateNever]
    public Cinema Cinema { get; set; }

    public string UserId { get; set; }
    [ValidateNever]
    public ApplicationUser User { get; set; }
    public DateTime ShowTime { get; set; }
    public ICollection<TicketSeats> TicketSeats { get; set; } = new List<TicketSeats>();

    public double Price { get; set; }
    public DateTime BookingDate { get; set; }

    [ValidateNever]
    public TicketStatus TicketStatus { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentStripeId { get; set; }
}
