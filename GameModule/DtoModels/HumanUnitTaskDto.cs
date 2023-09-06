using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanUnitTaskDto(int TribeId,
        int HumanUnitId,
        string HumanUnitName,
        EHumanUnitTaskType Type,
        DateTime From,
        DateTime To) : IHumanUnitTaskDto;

}
