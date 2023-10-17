using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Response;

namespace ProjectNomad.Client.Models.Requests
{
    internal record AddHumanUnitTaskOrder(int TribeId,
        ETaskType Type,
        int MapTileX,
        int MapTileY) : IAddHumanUnitTaskOrderDto;
}
