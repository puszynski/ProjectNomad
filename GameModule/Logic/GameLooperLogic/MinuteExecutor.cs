using GameModule.Entities;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IMinuteExecutor
    {
        void Execute(Tribe tribe, List<INotification> notifications);
    }


    internal class MinuteExecutor : IMinuteExecutor
    {
        readonly IHumansDeathApplicator _humansDeathApplicator;
        public MinuteExecutor(IHumansDeathApplicator humansDeathApplicator)
        {
            _humansDeathApplicator = humansDeathApplicator;
        }

        void IMinuteExecutor.Execute(Tribe tribe, List<INotification> notifications)
        {
            foreach (var human in tribe.HumanUnits)
                human.FoodLevelPercentage -= GameSETTINGS.Food.FoodToGetHungryForHumanUnitEachMinute;

            var notificationsAbouStarvations = _humansDeathApplicator.StarvationDeath(tribe.HumanUnits);
            notifications.AddRange(notificationsAbouStarvations);
        }
    }
}
