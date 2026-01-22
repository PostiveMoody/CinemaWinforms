using Cinema.Infrastructure.Exceptions;
using Cinema.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Cinema.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork<CinemaDbContext>
    {
        private readonly CinemaDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        private IMovieRepository _movies;
        private ISessionRepository _sessions;
        private ITicketRepository _tickets;

        public UnitOfWork(CinemaDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public CinemaDbContext Context => _context;

        // Репозитории с ленивой загрузкой
        public IMovieRepository Movies =>
            _movies ??= new MovieRepository(_context);

        public ISessionRepository Sessions =>
            _sessions ??= new SessionRepository(_context);

        public ITicketRepository Tickets =>
            _tickets ??= new TicketRepository(_context);

        public void SaveChanges(CancellationToken cancellationToken = default)
        {
            this._context.SaveChanges();
        }

        // Сохранение изменений
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Сохраняем изменения
                var result = await _context.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Обработка конфликтов параллельного доступа
                throw new ConcurrencyException("Конфликт параллельного доступа к данным", ex);
            }
            catch (DbUpdateException ex)
            {
                // Обработка ошибок БД
                throw new PersistenceException("Ошибка сохранения данных", ex);
            }
        }

        // Транзакции
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Транзакция уже начата");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Нет активной транзакции");
            }

            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Нет активной транзакции");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
                DetachAllEntities();
            }
        }


        // Управление отслеживанием сущностей
        public void DetachAllEntities()
        {
            var entries = _context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }


        // Dispose паттерн
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context?.Dispose();

                    // Dispose репозиториев
                    (_movies as IDisposable)?.Dispose();
                    (_sessions as IDisposable)?.Dispose();
                    (_tickets as IDisposable)?.Dispose();
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}

