using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanTaskDto(int Id,
        int TribeId,
        int HumanUnitId,
        string HumanUnitName,
        ETaskType Type,
        DateTime From,
        DateTime To,
        int LocalizationX,
        int LocalizationY) : IHumanTaskDto;

}
