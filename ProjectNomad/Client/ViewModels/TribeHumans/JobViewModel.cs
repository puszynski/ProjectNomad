using ProjectNomad.Shared.DTOs.ServerToWasm;
using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Client.ViewModels.TribeHumans
{
    public class JobViewModel
    {
        public JobViewModel(JobDto jobDto)
        {
            Id = jobDto.Id;
            Type = jobDto.Type;
            PriorityPercentage = jobDto.PriorityPercentage;
        }

        public int Id { get; private set; }
        public ETaskType Type { get; private set; }
        public int PriorityPercentage { get; private set; }


        public const int FULL_JOB_POINTS_SCALE = 5;
        public const int ONE_POINT_TO_PERCENTAGE_COEFFICIENT = 20;

        public void IncreaseJobPriority()
        {
            if (PriorityPercentage <= 100 - ONE_POINT_TO_PERCENTAGE_COEFFICIENT) { }
                PriorityPercentage += ONE_POINT_TO_PERCENTAGE_COEFFICIENT;
        }
        public void DecreaseJobPriority()
        {
            if (PriorityPercentage > ONE_POINT_TO_PERCENTAGE_COEFFICIENT)
                PriorityPercentage -= ONE_POINT_TO_PERCENTAGE_COEFFICIENT;
        }

        public static implicit operator JobViewModel(JobDto jobDto)
            => new JobViewModel(jobDto);

        public static implicit operator JobDto(JobViewModel model)
            => new JobDto(model.Id, model.Type, model.PriorityPercentage);
    }
}
