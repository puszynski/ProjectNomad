using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;

namespace GameModule.Repositories
{
    internal interface IJobRepository : IRepository
    {
        Task<Job> Add(int humanId, ETaskType type, int priorityPercentage);
        Task<Job> GetById(int id);
        Task Remove(int jobId);
        Task RemoveAll(int tribeId);
    }

    internal class JobRepository : BaseRepository, IJobRepository
    {
        public JobRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<Job> IJobRepository.Add(int humanId,
            ETaskType type,
            int priorityPercentage)
        {
            var model = new Job
            {
                HumanId = humanId,
                Type = type,
                PriorityPercentage = priorityPercentage
            };

            await _gameModuleDbContext.Jobs.AddAsync(model);
            return model;
        }

        async Task<Job> IJobRepository.GetById(int id)
            => await _gameModuleDbContext.Jobs.SingleOrDefaultAsync(x => x.Id == id);

        async Task IJobRepository.Remove(int jobId)
        {
            var job = await _gameModuleDbContext.Jobs.SingleAsync(x => x.Id == jobId);
            _gameModuleDbContext.Remove(job);
        }

        async Task IJobRepository.RemoveAll(int tribeId)
        => await _gameModuleDbContext.Jobs.Where(x => x.Human.TribeId == tribeId).ExecuteDeleteAsync();
    }
}
