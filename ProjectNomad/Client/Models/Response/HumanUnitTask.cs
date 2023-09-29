using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record HumanUnitTask(int TribeId,
        int HumanUnitId,
        string HumanUnitName,
        EHumanUnitTaskType Type,
        DateTime From,
        DateTime To) : IHumanUnitTaskDto;
}
