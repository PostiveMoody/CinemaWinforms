using Cinema.Domain.Entities;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public CinemaDbContext dbContext;

        public SessionRepository(CinemaDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbSet<Session> Sessions()
        {
            return this.dbContext.Sessions;
        }

        public void AddToContext(Session session)
        {
            this.dbContext.Add(session);
        }
    }
}
