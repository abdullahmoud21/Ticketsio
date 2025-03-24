namespace Ticketsio.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } 
        public bool IsBooked { get; set; }
        public bool IsConfirmed { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public ICollection<TicketSeats> TicketSeats { get; set; }
    }
}
