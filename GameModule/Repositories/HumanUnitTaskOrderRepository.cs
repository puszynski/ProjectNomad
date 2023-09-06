using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskOrderRepository : IRepository
    {
        Task RemoveAll(int tribeId);
        Task<List<HumanUnitTaskOrder>> Get(int tribeId);
        Task Add(int tribeId, EHumanUnitTaskType type, int mapTileId, DateTime now);
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


        async Task IHumanUnitTaskOrderRepository.Add(int tribeId, 
            EHumanUnitTaskType type, 
            int mapTileId, 
            DateTime now)
        {
            await _gameModuleDbContext.HumanUnitTaskOrders.AddAsync(new HumanUnitTaskOrder
            {
                Added = now,
                TribeId = tribeId,
                Type = type,
                IsInProgress = false,
                MapTileId = mapTileId
            });
        }
    }
}
