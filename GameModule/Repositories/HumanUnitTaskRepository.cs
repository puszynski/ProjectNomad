using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskRepository : IRepository
    {
        Task<List<HumanUnitTask>> GetHumanUnitTasksByTribeId(int tribeId);
        Task AddAsync(HumanUnitTask humanUnitTask);
        void Remove(HumanUnitTask humanUnitTask);
    }

    internal class HumanUnitTaskRepository : BaseRepository, IHumanUnitTaskRepository
    {
        public HumanUnitTaskRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<HumanUnitTask>> IHumanUnitTaskRepository.GetHumanUnitTasksByTribeId(int tribeId)
        {
            return await _gameModuleDbContext
                .HumanUnitTasks
                .Where(x => x.TribeId == tribeId)
                .ToListAsync();
        }

        async Task IHumanUnitTaskRepository.AddAsync(HumanUnitTask humanUnitTask)
        {
            await _gameModuleDbContext.AddAsync(humanUnitTask);
        }

        void IHumanUnitTaskRepository.Remove(HumanUnitTask humanUnitTask)
        {
            _gameModuleDbContext.Remove(humanUnitTask);
        }
    }
}
