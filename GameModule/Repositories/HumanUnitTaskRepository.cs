using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskRepository : IRepository
    {
        Task<List<HumanTask>> GetHumanUnitTasksByTribeId(int tribeId);
        Task AddAsync(HumanTask humanUnitTask);
        void Remove(HumanTask humanUnitTask);
        void RemoveRange(IEnumerable<HumanTask> humanUnitTask);
        Task RemoveAll(int tribeId);
    }

    internal class HumanUnitTaskRepository : BaseRepository, IHumanUnitTaskRepository
    {
        public HumanUnitTaskRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<HumanTask>> IHumanUnitTaskRepository.GetHumanUnitTasksByTribeId(int tribeId)
        {
            return await _gameModuleDbContext
                .HumanTasks
                .Where(x => x.TribeId == tribeId)
                .ToListAsync();
        }

        async Task IHumanUnitTaskRepository.RemoveAll(int tribeId)
            => await _gameModuleDbContext.HumanTasks.Where(x => x.TribeId == tribeId).ExecuteDeleteAsync();

        async Task IHumanUnitTaskRepository.AddAsync(HumanTask humanUnitTask)
        {
            await _gameModuleDbContext.HumanTasks.AddAsync(humanUnitTask);
        }

        void IHumanUnitTaskRepository.Remove(HumanTask humanUnitTask)
        {
            _gameModuleDbContext.HumanTasks.Remove(humanUnitTask);
        }

        public void RemoveRange(IEnumerable<HumanTask> humanUnitTask)
        {
            _gameModuleDbContext.HumanTasks.RemoveRange(humanUnitTask);
        }
    }
}
