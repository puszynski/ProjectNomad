using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IHumanUnitTaskRepository : IRepository
    {
        Task<List<HumanUnitTask>> GetHumanUnitTasksByTribeId(int tribeId);
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
    }
}
