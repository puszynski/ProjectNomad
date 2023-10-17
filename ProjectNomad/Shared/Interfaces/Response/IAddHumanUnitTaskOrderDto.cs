using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces.Response
{
    public interface IAddHumanUnitTaskOrderDto
    {
        public int TribeId { get; }
        public ETaskType Type { get; }
        public int MapTileX { get; }
        public int MapTileY { get; }

    }
}
