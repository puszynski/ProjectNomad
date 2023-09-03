using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Response;

namespace ProjectNomad.Server.Models.RequestModels
{
    public record AddHumanUnitTaskOrder(int TribeId,
        EHumanUnitTaskType Type,
        int MapTileX,
        int MapTileY) : IAddHumanUnitTaskOrderDto;
}
