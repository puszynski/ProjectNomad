using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IHumanUnitRepository : IRepository
    {
        Task<List<Human>> GetHumanUnitsByTribeId(int tribeId);
        void RemoveRange(IEnumerable<Human> humanUnits);
    }

    internal class HumanUnitRepository : BaseRepository, IHumanUnitRepository
    {
        public HumanUnitRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<Human>> IHumanUnitRepository.GetHumanUnitsByTribeId(int tribeId)
        {
            return await _gameModuleDbContext
                .Humans
                .Where(x => x.TribeId == tribeId)
                .ToListAsync();
        }

        void IHumanUnitRepository.RemoveRange(IEnumerable<Human> humanUnits)
        {
            _gameModuleDbContext.RemoveRange(humanUnits);
        }
    }
}
