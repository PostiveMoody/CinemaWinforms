using Cinema.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Data
{
    /// <summary>
    /// Unit of Work (Единица работы) — это паттерн, который группирует несколько операций с базой данных в одну логическую транзакцию. 
    /// Он обеспечивает согласованность данных и управляет отслеживанием изменений объектов.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Репозитории
        IMovieRepository Movies { get; }
        ISessionRepository Sessions { get; }
        ITicketRepository Tickets { get; }

        // Основные методы
        void SaveChanges(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // Транзакции
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();


        // Отслеживание изменений
        void DetachAllEntities();
    }

    // Generic версия для разных контекстов
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
