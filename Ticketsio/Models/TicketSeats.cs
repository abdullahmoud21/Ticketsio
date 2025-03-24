namespace Ticketsio.Models
{
    public class TicketSeats
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

    }
}
