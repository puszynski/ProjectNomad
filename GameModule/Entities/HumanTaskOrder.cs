using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;

namespace GameModule.Entities
{
    /// <summary>
    /// Rules: 
    /// 
    /// HumanTaskOrders are scheduling HumanTasks to be assigned each hour.
    /// 
    /// After ending, HumanTask is not deleted
    /// It wait until even hour - then all done HumanTask are cleared 
    /// Clearing makes room for next batch of HumanTasks for next hour
    /// 
    /// </summary>
    
    /// V2 - todo - will be used only as one time order
    internal class HumanTaskOrder : IId, ITribeReference, IAdded
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        //public int? HumanId { get; set; } //todo - to allow aiming concrete human 

        public DateTime Added { get; set; }
        public ETaskType Type { get; set; } 
        public Localization Localization { get; set; }
    }
}
