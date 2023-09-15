using GameModule.Configurations;
using GameModule.Entities;

namespace GameModule.Repositories
{
    internal interface ITribeRelocationRepository : IRepository
    {
        internal Task AddAsync(TribeRelocation tribeRelocation);
    }
    internal class TribeRelocationRepository : BaseRepository, ITribeRelocationRepository
    {
        public TribeRelocationRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task ITribeRelocationRepository.AddAsync(TribeRelocation tribeRelocation)
        {
            await _gameModuleDbContext.TribeRelocations.AddAsync(tribeRelocation);
        }
    }
}
