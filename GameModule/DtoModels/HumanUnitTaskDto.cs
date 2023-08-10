using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    public record HumanUnitTaskDto(int TribeId,
        int HumanUnitId,
        EHumanUnitTaskType Type,
        DateTime From,
        DateTime? To) : IHumanUnitTaskDto;

}
