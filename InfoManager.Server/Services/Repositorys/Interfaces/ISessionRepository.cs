using InfoManager.Server.Models;

namespace InfoManager.Server.Services.Repositorys.Interfaces
{
    public interface ISessionRepository
    {
        public Task AddAsync(Session session);
        public Task<Session?> FindAsync(string key);
        public Task LoadUserAsync(Session session);
    }
}
