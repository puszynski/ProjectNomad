using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskOrderRepository : IRepository
    {
        void Remove(HumanTaskOrder task);
        Task RemoveAll(int tribeId);
        Task<List<HumanTaskOrder>> Get(int tribeId);
        Task Add(int tribeId, ETaskType type, int X, int Y, DateTime now);
    }

    internal class HumanUnitTaskOrderRepository : BaseRepository, IHumanUnitTaskOrderRepository
    {
        public HumanUnitTaskOrderRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<HumanTaskOrder>> IHumanUnitTaskOrderRepository.Get(int tribeId)
            => await _gameModuleDbContext.HumanTaskOrders.Where(x => x.TribeId == tribeId).ToListAsync();

        void IHumanUnitTaskOrderRepository.Remove(HumanTaskOrder task)
            => _gameModuleDbContext.HumanTaskOrders.Remove(task);

        async Task IHumanUnitTaskOrderRepository.RemoveAll(int tribeId) 
            => await _gameModuleDbContext.HumanTaskOrders.Where(x => x.TribeId == tribeId).ExecuteDeleteAsync();


        async Task IHumanUnitTaskOrderRepository.Add(int tribeId, 
            ETaskType type, 
            int X,
            int Y,
            DateTime now)
        {
            await _gameModuleDbContext.HumanTaskOrders.AddAsync(new HumanTaskOrder
            {
                Added = now,
                TribeId = tribeId,
                Type = type,
                Localization = new Entities.ValueObjects.Localization { X = X, Y = Y }
            });
        }
    }
}
