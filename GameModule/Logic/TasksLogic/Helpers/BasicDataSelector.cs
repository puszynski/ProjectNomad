using GameModule.Entities;
using ProjectNomad.Shared;

namespace GameModule.Logic.TasksLogic.Helpers
{
    internal static class BasicDataSelector
    {
        internal static Human? SelectFirstHumanWithCondition(Tribe tribe)
        {
            return RandomCalculator.GetRandomItemFromList(SelectHumansWithCondition(tribe));
        }

        internal static IEnumerable<Human> SelectHumansWithCondition(Tribe tribe)
        {
            if (tribe.Humans == null)
                return default;

            var humanIDsWithTaskAssigned = tribe
                .HumanTasks?
                .Where(x => !x.IsCompleted)
                .Select(x => x.HumanId);

            if (humanIDsWithTaskAssigned == null)
                return tribe.Humans
                    .Where(x => x.FoodLevelPercentage > 10);


            return tribe.Humans
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage > 10);
        }
    }
}
