namespace Cinema.Domain.Entities
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationMinutes { get; set; }

        // Навигационное свойство для сеансов
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
