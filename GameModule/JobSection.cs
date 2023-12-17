using GameModule.Repositories;
using ProjectNomad.Shared.DTOs.ServerToWasm;
using ProjectNomad.Shared.Enums;

namespace GameModule
{
    public interface IJobSection
    {
        Task<JobDto> AddJob(int humanId, ETaskType type, int priorityPercentage);
        Task UpdateJob(int id, int priorityPercentage);
        Task DeleteJob(int id);
    }

    internal class JobSection : IJobSection
    {
        private readonly IJobRepository _jobRepository;
        public JobSection(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<JobDto> AddJob(int humanId, ETaskType type, int priorityPercentage)
        {
            //todo validation:
            //- task with given type not exist in db
            //- priorityPercentage with allowed value in relation to existing jobs 

            var entity = await _jobRepository.Add(humanId, type, priorityPercentage);
            await _jobRepository.SaveChangesAsync();

            return new JobDto(entity.Id, entity.Type, entity.PriorityPercentage);
        }

        public async Task UpdateJob(int id, int priorityPercentage)
        {
            //todo validation:
            //- priorityPercentage with allowed value in relation to existing jobs 

            var entity = await _jobRepository.GetById(id) ?? throw new ArgumentNullException();

            entity.PriorityPercentage = priorityPercentage;
            await _jobRepository.SaveChangesAsync();
        }

        public async Task DeleteJob(int id)
        {
            _ = await _jobRepository.GetById(id) ?? throw new ArgumentNullException();
            await _jobRepository.Remove(id);
            await _jobRepository.SaveChangesAsync();
        }
    }
}
