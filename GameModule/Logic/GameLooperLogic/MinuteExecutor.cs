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
        readonly IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;
        public MinuteExecutor(IHumanUnitRepository humanUnitRepository,
            IHumanUnitTaskRepository humanUnitTaskRepository,
            IHumanUnitTaskOrderRepository humanUnitTaskOrderRepository)
        {
            _humanUnitRepository = humanUnitRepository;
            _humanUnitTaskRepository = humanUnitTaskRepository;
            _humanUnitTaskOrderRepository = humanUnitTaskOrderRepository;
        }

        public bool Execute(List<HumanUnit> humanUnits, Tribe tribe, List<HumanUnitTask> tasksToConsume)
        {
            var shouldBreakGameLoop = false;

            humanUnits.ForEach(x => x.FoodLevelPercentage = x.FoodLevelPercentage - GameSETTINGS.FoodToGetHungryForHumanUnitEachMinute);
            DeathApplicator.StarvationDeath(_humanUnitTaskOrderRepository, _humanUnitRepository, humanUnits, tribe.Id);

            if (!humanUnits.Any())
            {
                TribeDeathApplicator.Execute(tasksToConsume, _humanUnitTaskRepository);
                shouldBreakGameLoop = true;
            }

            return shouldBreakGameLoop;
        }
    }
}
