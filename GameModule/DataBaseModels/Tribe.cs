using GameModule.DataBaseModels.ValueObjects;

namespace GameModule.DataBaseModels
{
    internal class Tribe
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int Name { get; set; }

        public Localization Localization { get; set; }

        ICollection<HumanUnit> HumanUnits { get; set; }
    }
}
