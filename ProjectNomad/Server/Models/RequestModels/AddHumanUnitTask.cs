using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.Models.RequestModels
{
    public record AddHumanUnitTask(int TribeId,
        int HumanUnitId,
        EHumanUnitTaskType Type,
        int LocalizationStart_X,
        int LocalizationStart_Y,
        int LocalizationEnd_X,
        int LocalizationEnd_Y) : IAddHumanUnitTaskDto;
}
