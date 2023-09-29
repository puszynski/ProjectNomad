using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record HumanUnitTaskOrder(int Id,
        int TribeId,
        DateTime Added,
        EHumanUnitTaskType Type,
        bool IsInProgress,
        int? X,
        int? Y) : IHumanUnitTaskOrder;
}
