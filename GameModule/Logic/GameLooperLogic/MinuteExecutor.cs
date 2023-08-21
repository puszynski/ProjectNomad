using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IMinuteExecutor
    {
        void Execute(List<HumanUnit> humanUnits, Tribe tribe);
    }

    internal class MinuteExecutor : IMinuteExecutor
    {
        readonly IHumanUnitRepository _humanUnitRepository;
        public MinuteExecutor(IHumanUnitRepository humanUnitRepository)
        {
            _humanUnitRepository = humanUnitRepository;
        }

        void IMinuteExecutor.Execute(List<HumanUnit> humanUnits, Tribe tribe)
        {
            humanUnits.ForEach(x => x.FoodLevelPercentage = x.FoodLevelPercentage - GameSETTINGS.FoodToGetHungryForHumanUnitEachMinute);
            DeathApplicator.StarvationDeath(_humanUnitRepository, humanUnits);
        }
    }
}
