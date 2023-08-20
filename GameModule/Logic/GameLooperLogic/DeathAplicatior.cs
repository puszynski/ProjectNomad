using GameModule.Configurations;
using GameModule.Entities;

namespace GameModule.Logic.GameLooperLogic
{
    internal static class DeathApplicator
    {
        internal static void StarvationDeath(GameModuleDbContext _dbContext,
            ICollection<HumanUnit> humanUnits)
        {
            var humansToDieFromStarving = humanUnits.Where(x => x.FoodLevelPercentage <= 0).ToList();

            if (humansToDieFromStarving.Any())
            {
                _dbContext.RemoveRange(humansToDieFromStarving);

                //humansToDieFromStarving.ToList().ForEach(x => CreateNotification(x, notificationModule)); what to do with notifications after moving it to WASM?

                foreach (var humanUnitRemoved in humansToDieFromStarving)
                        humanUnits.Remove(humanUnitRemoved);
            }
        }
    }
}
