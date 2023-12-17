using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.DTOs.ServerToWasm
{
    public record HumanWithJobsDto(HumanDto Human, IEnumerable<JobDto> Jobs);

    public record HumanDto(int Id, string Name, int FoodLevel, int ThermalLevel);

    public record JobDto(int Id, ETaskType Type, int PriorityPercentage);
}
