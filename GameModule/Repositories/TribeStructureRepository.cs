using GameModule.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface ITribeStructureRepository : IRepository
    {
        internal Task RemoveAll(int tribeId);
    }

    internal class TribeStructureRepository : BaseRepository, ITribeStructureRepository
    {
        public TribeStructureRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {            
        }

        async Task ITribeStructureRepository.RemoveAll(int tribeId)
                => await _gameModuleDbContext.TribeStructures.Where(x => x.TribeId == tribeId).ExecuteDeleteAsync();
    }
}
