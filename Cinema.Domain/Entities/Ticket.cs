namespace Cinema.Domain.Entities
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int SessionId { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int SeatType { get; set; }

        // Навигационные свойства
        public virtual Session Session { get; set; }

    }
}
