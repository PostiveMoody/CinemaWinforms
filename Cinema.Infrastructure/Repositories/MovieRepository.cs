using Cinema.Domain.Entities;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public CinemaDbContext dbContext;

        public MovieRepository(CinemaDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbSet<Movie> Movies()
        {
            return this.dbContext.Movies;
        }

        public void AddToContext(Movie movie)
        {
            this.dbContext.Add(movie);
        }
    }
}
