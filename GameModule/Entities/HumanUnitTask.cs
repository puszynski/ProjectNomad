using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Entities
{
    internal class HumanUnitTask : IId
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public int HumanUnitId { get; set; }
        public EHumanUnitTaskType Type { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int MapTileId { get; set; }
    }
}
