using InfoManager.Server.DbContexts;
using InfoManager.Server.Models;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace InfoManager.Server.Services.Repositorys
{
    public class SessionRepository : ISessionRepository
    {
        private readonly MainDbContext dbContext;

        public SessionRepository(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(Session session)
        {
            await dbContext.AddAsync(session);
        }

        public Task<Session?> FindAsync(string key)
        {

            var r = dbContext.Sessions.Where(x => x.Key == key);

            return r.FirstOrDefaultAsync();
        }

        public async Task LoadUserAsync(Session session)
        {
            await dbContext.Sessions.Entry(session)
                .Reference(x => x.User)
                .LoadAsync();
        }
    }
}
