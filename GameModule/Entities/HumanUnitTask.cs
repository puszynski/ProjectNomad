using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Properties;

namespace GameModule.Entities
{
    //todo rename to "HumanTask"
    internal class HumanUnitTask : IId, ITribeId
    {
        public int Id { get; set; }
        public int TribeId { get; set; }

        public int HumanUnitId { get; set; }
        public HumanUnit HumanUnit { get; set; }

        public EHumanUnitTaskType Type { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int? MapTileId { get; set; }

    }
}
