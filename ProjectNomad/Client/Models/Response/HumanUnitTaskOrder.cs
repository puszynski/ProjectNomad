using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record HumanUnitTaskOrder(int Id,
        int TribeId,
        DateTime Added,
        ETaskType Type,
        int? HumanTaskId,
        int? X,
        int? Y) : IHumanUnitTaskOrder;
}
