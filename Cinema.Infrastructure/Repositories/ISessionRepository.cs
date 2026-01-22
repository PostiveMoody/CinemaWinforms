using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public interface ISessionRepository : IRepository<Session>
    {
        /// <summary>
        /// Получение всех Сеансов
        /// </summary>
        /// <returns></returns>
        public DbSet<Session> Sessions();

        /// <summary>
        /// Метод создания нового сеанса
        /// </summary>
        /// <param name="requestCard"></param>
        public void AddToContext(Session session);


    }
}
