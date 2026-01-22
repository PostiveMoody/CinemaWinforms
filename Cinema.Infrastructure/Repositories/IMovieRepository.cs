using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        /// <summary>
        /// Получение всех Фильмов
        /// </summary>
        /// <returns></returns>
        public DbSet<Movie> Movies();

        /// <summary>
        /// Метод создания нового Фильма
        /// </summary>
        /// <param name="movie"></param>
        public void AddToContext(Movie movie);
    }
}
