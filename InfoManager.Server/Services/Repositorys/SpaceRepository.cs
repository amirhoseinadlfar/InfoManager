using InfoManager.Server.DbContexts;
using InfoManager.Server.Models;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace InfoManager.Server.Services.Repositorys
{
    public class SpaceRepository : ISpaceRepository
    {
        private readonly MainDbContext dbContext;

        public SpaceRepository(MainDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(Space space)
        {
            await dbContext.Spaces.AddAsync(space);
        }

        public Task<Space?> FindAsync(int id)
        {
            return dbContext.Spaces.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
