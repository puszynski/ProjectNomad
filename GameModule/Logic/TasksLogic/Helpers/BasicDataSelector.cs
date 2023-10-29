using GameModule.Entities;

namespace GameModule.Logic.TasksLogic.Helpers
{
    internal static class BasicDataSelector
    {
        internal static Human? SelectFirstHumanWithCondition(Tribe tribe)
        {
            var humanIDsWithTaskAssigned = tribe.HumanTasks.Where(x => !x.IsCompleted).Select(x => x.HumanId);

            var humanWithConditionToStartNewTask = tribe
                .Humans
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage > 10)
                .FirstOrDefault();//todo make random (always first guy will be taken..)

            return humanWithConditionToStartNewTask;
        }
    }
}
