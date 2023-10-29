using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;

namespace GameModule.Entities
{
    internal class HumanTask : IId, ITribeReference
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        public int HumanId { get; set; }
        public Human Human { get; set; }

        public ETaskType Type { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsCompleted { get; set; }

        public Localization? Localization { get; set; }
    }
}
