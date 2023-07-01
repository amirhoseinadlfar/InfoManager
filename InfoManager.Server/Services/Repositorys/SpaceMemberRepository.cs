using InfoManager.Server.DbContexts;
using InfoManager.Server.Models;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Text.Json;

namespace InfoManager.Server.Services.Repositorys
{
    public class SpaceMemberRepository : ISpaceMemberRepository
    {
        private readonly MainDbContext dbContext;

        public SpaceMemberRepository(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(SpaceMember spaceMember)
        {
            await dbContext.SpaceMembers.AddAsync(spaceMember);
        }

        public Task DeleteAsync(SpaceMember spaceMember)
        {
            dbContext.SpaceMembers.Remove(spaceMember);
            return Task.CompletedTask;
        }

        public Task<SpaceMember?> FindAsync(Space space, User user)
        {
            return dbContext.SpaceMembers.Where(x => x.SpaceId == space.Id && x.UserId == user.Id)
                .FirstOrDefaultAsync();
        }
    }
}
