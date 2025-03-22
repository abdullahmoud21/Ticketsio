namespace Ticketsio.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } 
        public bool IsBooked { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
