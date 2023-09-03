using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Response;

namespace ProjectNomad.Client.DtoModels
{
    internal record AddHumanUnitTaskOrderRequest(int TribeId, 
        EHumanUnitTaskType Type, 
        int MapTileX,
        int MapTileY) : IAddHumanUnitTaskOrderDto;
}
