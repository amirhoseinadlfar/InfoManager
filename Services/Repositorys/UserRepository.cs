using InfoManager.DbContexts;
using InfoManager.Models;
using InfoManager.Services.Repositorys.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace InfoManager.Services.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDbContext dbContext;

        public UserRepository(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
        }

        public async Task<bool> AnyAsync(string username)
        {
            return await dbContext.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task<User> FindAsync(int id)
        {
            return await dbContext.Users.FirstAsync(x => x.Id == id);
        }

        public async Task<User> FindAsync(string username, string password)
        {
            return await dbContext.Users.FirstAsync(x=>x.UserName == username && x.Password == password);
        }

        public async Task SaveAsync(User user)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
