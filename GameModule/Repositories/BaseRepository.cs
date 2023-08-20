using GameModule.Configurations;

namespace GameModule.Repositories
{
    internal interface IRepository
    {
        Task<int> SaveChangesAsync();
    }

    internal class BaseRepository
    {
        protected readonly GameModuleDbContext _gameModuleDbContext;
        protected BaseRepository(GameModuleDbContext gameModuleDbContext)
        {
            _gameModuleDbContext = gameModuleDbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _gameModuleDbContext.SaveChangesAsync();
        }
    }
}
