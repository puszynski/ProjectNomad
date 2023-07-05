using GameModule.Configurations;
using GameModule.Entities;

namespace GameModule.Logic.GameLooperLogic
{
    internal static class DeathApplicator
    {
        internal static void StarvationDeath(GameModuleDbContext _dbContext,
            ICollection<HumanUnit> humanUnits)
        {
            var humansToDieFromStarving = humanUnits.Where(x => x.FoodLevelPercentage <= 0);

            if (humansToDieFromStarving.Any())
                _dbContext.RemoveRange(humansToDieFromStarving);
        }
    }
}
