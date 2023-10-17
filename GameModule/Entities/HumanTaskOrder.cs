using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;

namespace GameModule.Entities
{
    internal class HumanTaskOrder : IId, ITribeReference
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        public DateTime Added { get; set; }
        public ETaskType Type { get; set; } 
        public Localization Localization { get; set; }

        public int? HumanTaskId { get; set; }
        public HumanTask? HumanTask { get; set; }
    }
}
