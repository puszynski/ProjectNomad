using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.DtoModels
{
    public record HumanUnitTaskOrder(int Id,
        int TribeId,
        DateTime Added,
        EHumanUnitTaskType Type,
        bool IsInProgress,
        int? MapTileId) : IHumanUnitTaskOrder;
}
