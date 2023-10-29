using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanUnitTaskDto(int Id,
        int TribeId,
        int HumanUnitId,
        string HumanUnitName,
        ETaskType Type,
        DateTime From,
        DateTime To,
        bool IsCompleted) : IHumanUnitTaskDto;

}
