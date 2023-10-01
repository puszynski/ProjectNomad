using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;

namespace GameModule.Entities
{
    internal class TribeRelocation : IId, ITribeReference
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Localization Start { get; set; }
        public Localization Destiny { get; set; }
    }
}