using InfoManager.Server.DbContexts;
using InfoManager.Server.Models;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace InfoManager.Server.Services.Repositorys
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

        public async Task<User?> FindAsync(int id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> FindAsync(string username, string password)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x=>x.UserName == username && x.Password == password);
        }

        public async Task LoadSpaces(User user)
        {
            await dbContext.Entry(user)
                .Collection(x => x.Spaces)
                .LoadAsync();
        }
    }
}
