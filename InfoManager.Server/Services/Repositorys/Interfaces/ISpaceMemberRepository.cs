using InfoManager.Server.Models;

namespace InfoManager.Server.Services.Repositorys.Interfaces
{
    public interface ISpaceMemberRepository
    {
        Task AddAsync(SpaceMember spaceMember);
        Task DeleteAsync(SpaceMember spaceMember);
        Task<SpaceMember?> FindAsync(Space space, User user);
    }
}
