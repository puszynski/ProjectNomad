using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanUnitTaskOrderDto(int Id, 
        int TribeId, 
        DateTime Added, 
        EHumanUnitTaskType Type, 
        bool IsInProgress,
        int? MapTileId) : IHumanUnitTaskOrder;
}
