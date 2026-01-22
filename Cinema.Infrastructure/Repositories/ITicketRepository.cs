using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        /// <summary>
        /// Получение всех Билетов
        /// </summary>
        /// <returns></returns>
        DbSet<Ticket> Tickets();

        /// <summary>
        /// Метод создания нового билета
        /// </summary>
        /// <param name="requestCard"></param>
        Task AddToContextAsync(Ticket ticket);

        Task<bool> IsSeatTakenAsync(int sessionId, int row, int seat, CancellationToken cancellationToken = default);
    }
}
