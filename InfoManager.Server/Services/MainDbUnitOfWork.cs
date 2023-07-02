using InfoManager.Server.DbContexts;
using InfoManager.Server.Services.Repositorys.Interfaces;

namespace InfoManager.Server.Services
{
    public class MainDbUnitOfWork
    {
        private readonly MainDbContext mainDbContext;

        public MainDbUnitOfWork(MainDbContext mainDbContext, IUserRepository userRepository, ISessionRepository sessionRepository, ISpaceRepository spaceRepository, ISpaceMemberRepository spaceMemberRepository, ITableRepository tableRepository)
        {
            this.mainDbContext = mainDbContext;
            UserRepository = userRepository;
            SessionRepository = sessionRepository;
            SpaceRepository = spaceRepository;
            SpaceMemberRepository = spaceMemberRepository;
            TableRepository = tableRepository;
        }

        public IUserRepository UserRepository { get; }
        public ISessionRepository SessionRepository { get; }
        public ISpaceRepository SpaceRepository { get; }
        public ISpaceMemberRepository SpaceMemberRepository { get; }
        public ITableRepository TableRepository { get; }

        public Task SaveChangesAsync()
        {
            return mainDbContext.SaveChangesAsync();
        }
    }
}
