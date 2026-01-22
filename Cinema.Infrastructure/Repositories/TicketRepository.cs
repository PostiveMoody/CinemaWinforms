using Cinema.Domain.Entities;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public CinemaDbContext dbContext;

        public TicketRepository(CinemaDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbSet<Ticket> Tickets()
        {
            return this.dbContext.Tickets;
        }

        public async Task AddToContextAsync(Ticket ticket)
        {
            await this.dbContext.AddAsync(ticket);
        }

        public async Task<bool> IsSeatTakenAsync(int sessionId, int row, int seat, CancellationToken cancellationToken = default)
        {
            var isSeatTaken = await this.dbContext.Tickets
                .FirstOrDefaultAsync(tiket => tiket.SessionId == sessionId && tiket.RowNumber == row && tiket.SeatNumber == seat);

            if (isSeatTaken == null)
                return false;
            else
                return true;
        }
    }
}
