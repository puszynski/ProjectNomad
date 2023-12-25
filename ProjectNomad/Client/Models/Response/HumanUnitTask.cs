using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record HumanUnitTask(int Id,
        int TribeId,
        int HumanUnitId,
        string HumanUnitName,
        ETaskType Type,
        DateTime From,
        DateTime To,
        bool IsCompleted,
        int LocalizationX,
        int LocalizationY) : IHumanTaskDto;
}
