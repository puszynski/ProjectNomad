using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Interfaces.Properties;

namespace GameModule.Entities
{
    internal class TribeRelocation : IId
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