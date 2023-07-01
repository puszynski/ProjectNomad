using GameModule.Entities.ValueObjects;

namespace GameModule.Entities
{
    internal class HumanUnit
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }
        public string Name { get; set; }
        public Localization Localization { get; set; }

        //1-100
        public int FoodLevelPercentage { get; set; }
    }
}
