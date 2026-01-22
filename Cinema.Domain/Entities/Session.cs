namespace Cinema.Domain.Entities
{
    public class Session
    {
        public int SessionId { get; set; }
        public int MovieId { get; set; }
        public DateTime DateTime { get; set; }
        public int HallNumber { get; set; }

        // Навигационные свойства
        public virtual Movie Movie { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

}
