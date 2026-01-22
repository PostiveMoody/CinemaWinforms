using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Data
{
    public class CinemaDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // Конструктор для миграций
        public CinemaDbContext() { }

        // Конструктор с параметрами для DI
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options) 
        { 
        }

        // DbSet для сущностей
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureMovie(modelBuilder);
            ConfigureSession(modelBuilder);
            ConfigureTicket(modelBuilder);

            //// Заполнение начальными данными
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=Cinema;Integrated Security=True;Trust Server Certificate=True",
                    options => options.EnableRetryOnFailure(3));

                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }

        private void ConfigureMovie(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Movie>().HasKey("MovieId");
            modelBuilder.Entity<Movie>().Property(it => it.MovieId).HasDefaultValueSql("NEXT VALUE FOR MovieIdSequence");
            modelBuilder.HasSequence<int>("MovieIdSequence").IncrementsBy(1).HasMin(1).HasMax(100000).StartsAt(1);
        }

        private void ConfigureSession(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<Session>().HasKey("SessionId");
            modelBuilder.Entity<Session>().Property(it => it.SessionId).HasDefaultValueSql("NEXT VALUE FOR SessionIdSequence");
            modelBuilder.HasSequence<int>("SessionIdSequence").IncrementsBy(1).HasMin(1).HasMax(100000).StartsAt(1);

            modelBuilder.Entity<Session>(entity =>
            {
                // Внешний ключ к Movie
                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.Sessions)
                    .HasForeignKey(e => e.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureTicket(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<Ticket>().HasKey("TicketId");
            modelBuilder.Entity<Ticket>().Property(it => it.TicketId).HasDefaultValueSql("NEXT VALUE FOR TicketIdSequence");
            modelBuilder.HasSequence<int>("TicketIdSequence").IncrementsBy(1).HasMin(1).HasMax(100000).StartsAt(1);

            modelBuilder.Entity<Ticket>(entity =>
            {
                // Внешний ключ к Session
                entity.HasOne(e => e.Session)
                    .WithMany(s => s.Tickets)
                    .HasForeignKey(e => e.SessionId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Начальные данные для тестирования
            var movies = new[]
            {
                new Movie
                {
                    MovieId = 1,
                    Title = "Интерстеллар",
                    Genre = "Фантастика",
                    DurationMinutes = 169
                },
                new Movie
                {
                    MovieId = 2,
                    Title = "Крестный отец",
                    Genre = "Криминал",
                    DurationMinutes = 175
                },
                new Movie
                {
                    MovieId = 3,
                    Title = "Побег из Шоушенка",
                    Genre = "Драма",
                    DurationMinutes = 142
                }
            };

            var sessions = new[]
            {
                new Session
                {
                    SessionId = 1,
                    MovieId = 1,
                    DateTime = DateTime.Today.AddHours(18),
                    HallNumber = 1
                },
                new Session
                {
                    SessionId = 2,
                    MovieId = 1,
                    DateTime = DateTime.Today.AddHours(21).AddMinutes(30),
                    HallNumber = 1
                },
                new Session
                {
                    SessionId = 3,
                    MovieId = 2,
                    DateTime = DateTime.Today.AddHours(19),
                    HallNumber = 2
                }
            };

            var tickets = new[]
            {
                new Ticket
                {
                    TicketId = 1,
                    SessionId = 1,
                    RowNumber = 5,
                    SeatNumber = 10,
                    Price = 600.00m,
                    IsAvailable = true,
                    SeatType = 2
                },
                new Ticket
                {
                    TicketId = 2,
                    SessionId = 1,
                    RowNumber = 5,
                    SeatNumber = 11,
                    Price = 450.00m,
                    IsAvailable = false,
                    SeatType = 0
                }
            };

            modelBuilder.Entity<Movie>().HasData(movies);
            modelBuilder.Entity<Session>().HasData(sessions);
            modelBuilder.Entity<Ticket>().HasData(tickets);
        }


    }
}
