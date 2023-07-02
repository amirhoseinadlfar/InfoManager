using InfoManager.Server.Models;

namespace InfoManager.Server.Services.Repositorys.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task<User?> FindAsync(int id);
        public Task<User?> FindAsync(string username,string password);
        public Task<bool> AnyAsync(string username);
        Task LoadSpaces(User user);
    }
}
