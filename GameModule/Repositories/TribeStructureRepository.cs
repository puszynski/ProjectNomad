using GameModule.Configurations;

namespace GameModule.Repositories
{
    internal interface ITribeStructureRepository : IRepository
    {
    }

    internal class TribeStructureRepository : BaseRepository, ITribeStructureRepository
    {
        public TribeStructureRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }
    }
}
