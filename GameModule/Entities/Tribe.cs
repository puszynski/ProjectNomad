using GameModule.Entities.ValueObjects;

namespace GameModule.Entities
{
    internal class Tribe
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }//todo migration
        public string Name { get; set; }//todo migration

        //todo LastRecalculationDate / UpdatedDate - to get time to recalculate data

        public Localization Localization { get; set; }

        ICollection<HumanUnit> HumanUnits { get; set; }
    }
}
