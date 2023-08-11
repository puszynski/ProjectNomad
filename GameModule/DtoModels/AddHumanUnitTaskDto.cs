using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    public record AddHumanUnitTaskDto(int TribeId, 
        int HumanUnitId, 
        EHumanUnitTaskType Type, 
        int LocalizationStart_X,
        int LocalizationStart_Y,
        int LocalizationEnd_X,
        int LocalizationEnd_Y) : IAddHumanUnitTaskDto;   
}
