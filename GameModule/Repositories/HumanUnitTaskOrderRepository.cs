using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskOrderRepository : IRepository
    {
        Task RemoveAll(int tribeId);
        Task<List<HumanUnitTaskOrder>> Get(int tribeId);
    }

    internal class HumanUnitTaskOrderRepository : BaseRepository, IHumanUnitTaskOrderRepository
    {
        public HumanUnitTaskOrderRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<HumanUnitTaskOrder>> IHumanUnitTaskOrderRepository.Get(int tribeId)
            => await _gameModuleDbContext.HumanUnitTaskOrders.Where(x => x.TribeId == tribeId).ToListAsync();

        async Task IHumanUnitTaskOrderRepository.RemoveAll(int tribeId) 
            => await _gameModuleDbContext.HumanUnitTaskOrders.Where(x => x.TribeId == tribeId).ExecuteDeleteAsync();
    }
}
