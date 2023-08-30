using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IMinuteExecutor
    {
        bool Execute(List<HumanUnit> humanUnits, 
            Tribe tribe, 
            List<HumanUnitTask> tasksToConsume);
    }

    internal class MinuteExecutor : IMinuteExecutor
    {
        readonly IHumanUnitRepository _humanUnitRepository;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        public MinuteExecutor(IHumanUnitRepository humanUnitRepository, 
            IHumanUnitTaskRepository humanUnitTaskRepository)
        {
            _humanUnitRepository = humanUnitRepository;
            _humanUnitTaskRepository = humanUnitTaskRepository;
        }

        public bool Execute(List<HumanUnit> humanUnits, Tribe tribe, List<HumanUnitTask> tasksToConsume)
        {
            var shouldBreakGameLoop = false;

            humanUnits.ForEach(x => x.FoodLevelPercentage = x.FoodLevelPercentage - GameSETTINGS.FoodToGetHungryForHumanUnitEachMinute);
            DeathApplicator.StarvationDeath(_humanUnitRepository, humanUnits);

            if (!humanUnits.Any())
            {
                TribeDeathApplicator.Execute(tasksToConsume, _humanUnitTaskRepository);
                shouldBreakGameLoop = true;
            }

            return shouldBreakGameLoop;
        }
    }
}
