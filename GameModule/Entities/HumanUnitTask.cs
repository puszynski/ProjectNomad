using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;

namespace GameModule.Entities
{
    //todo rename to "HumanTask"
    internal class HumanUnitTask : IId, ITribeReference
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        public int HumanUnitId { get; set; }
        public HumanUnit HumanUnit { get; set; }

        public EHumanUnitTaskType Type { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Localization? Localization { get; set; }
        //public int? MapTileId { get; set; }

    }
}
