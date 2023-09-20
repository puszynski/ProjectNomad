using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskOrderRepository : IRepository
    {
        void Remove(HumanUnitTaskOrder task);
        Task RemoveAll(int tribeId);
        Task<List<HumanUnitTaskOrder>> Get(int tribeId);
        Task Add(int tribeId, EHumanUnitTaskType type, int X, int Y, DateTime now);
    }

    internal class HumanUnitTaskOrderRepository : BaseRepository, IHumanUnitTaskOrderRepository
    {
        public HumanUnitTaskOrderRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<HumanUnitTaskOrder>> IHumanUnitTaskOrderRepository.Get(int tribeId)
            => await _gameModuleDbContext.HumanUnitTaskOrders.Where(x => x.TribeId == tribeId).ToListAsync();

        void IHumanUnitTaskOrderRepository.Remove(HumanUnitTaskOrder task)
            => _gameModuleDbContext.HumanUnitTaskOrders.Remove(task);

        async Task IHumanUnitTaskOrderRepository.RemoveAll(int tribeId) 
            => await _gameModuleDbContext.HumanUnitTaskOrders.Where(x => x.TribeId == tribeId).ExecuteDeleteAsync();


        async Task IHumanUnitTaskOrderRepository.Add(int tribeId, 
            EHumanUnitTaskType type, 
            int X,
            int Y,
            DateTime now)
        {
            await _gameModuleDbContext.HumanUnitTaskOrders.AddAsync(new HumanUnitTaskOrder
            {
                Added = now,
                TribeId = tribeId,
                Type = type,
                IsInProgress = false,
                Localization = new Entities.ValueObjects.Localization { X = X, Y = Y }
            });
        }
    }
}
