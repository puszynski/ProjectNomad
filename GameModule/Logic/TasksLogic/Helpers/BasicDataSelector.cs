using GameModule.Entities;
using ProjectNomad.Shared.Enums;

namespace GameModule.Logic.TasksLogic.Helpers
{
    internal static class BasicDataSelector
    {
        internal static HumanUnit? SelectFirstHumanWithCondition(Tribe tribe)
        {
            var humanIDsWithTaskAssigned = tribe.HumanUnitTasks.Select(x => x.HumanUnitId);

            var humanWithConditionToStartNewTask = tribe
                .HumanUnits
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage > 10)
                .FirstOrDefault();//todo make random (always first guy will be taken..)

            return humanWithConditionToStartNewTask;
        }

        internal static HumanUnitTaskOrder? GetEndingTaskOrder(ICollection<HumanUnitTaskOrder> humanUnitTaskOrders,
            EHumanUnitTaskType type)
        {
            return humanUnitTaskOrders
                    .Where(x => x.Type == type && x.IsInProgress)
                    .OrderBy(x => x.Added)
                    .FirstOrDefault();
        }
    }
}
