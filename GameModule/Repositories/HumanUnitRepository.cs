using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IHumanUnitRepository : IRepository
    {
        Task<List<HumanUnit>> GetHumanUnitsByTribeId(int tribeId);
        void RemoveRange(IEnumerable<HumanUnit> humanUnits);
    }

    internal class HumanUnitRepository : BaseRepository, IHumanUnitRepository
    {
        public HumanUnitRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<HumanUnit>> IHumanUnitRepository.GetHumanUnitsByTribeId(int tribeId)
        {
            return await _gameModuleDbContext
                .HumanUnits
                .Where(x => x.TribeId == tribeId)
                .ToListAsync();
        }

        void IHumanUnitRepository.RemoveRange(IEnumerable<HumanUnit> humanUnits)
        {
            _gameModuleDbContext.RemoveRange(humanUnits);
        }
    }
}
