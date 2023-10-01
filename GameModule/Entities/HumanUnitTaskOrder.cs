using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;

namespace GameModule.Entities
{
    //todo name "TaskOrder" or "TribeTaskOrder"
    internal class HumanUnitTaskOrder : IId, ITribeReference
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }
        public DateTime Added { get; set; }
        public EHumanUnitTaskType Type { get; set; } 
        public bool IsInProgress { get; set; }
        public Localization Localization { get; set; }
    }
}
