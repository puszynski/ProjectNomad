using GameModule.Entities;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IMinuteExecutor
    {
        List<INotification> Execute(List<HumanUnit> humanUnits, 
            Tribe tribe, 
            List<HumanUnitTask> tasksToConsume);
    }

    internal class MinuteExecutor : IMinuteExecutor
    {
        readonly IGameOverApplicator _gameOverApplicator;
        readonly IHumansDeathApplicator _humansDeathApplicator;
        public MinuteExecutor(IGameOverApplicator gameOverApplicator,
            IHumansDeathApplicator humansDeathApplicator)
        {
            _gameOverApplicator = gameOverApplicator;
            _humansDeathApplicator = humansDeathApplicator;
        }

        public List<INotification> Execute(List<HumanUnit> humanUnits, Tribe tribe, List<HumanUnitTask> tasksToConsume)
        {
            var notifications = new List<INotification>();

            humanUnits.ForEach(x => x.FoodLevelPercentage = x.FoodLevelPercentage - GameSETTINGS.FoodToGetHungryForHumanUnitEachMinute);
            notifications.AddRange(_humansDeathApplicator.StarvationDeath(humanUnits));

            return notifications;
        }
    }
}
