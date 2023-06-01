using InfoManager.Models;

namespace InfoManager.Services.Repositorys.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task<User> FindAsync(int id);
        public Task<bool> AnyAsync(string username);
        public Task<User> FindAsync(string username,string password);
        public Task SaveAsync(User user);
    }
}
