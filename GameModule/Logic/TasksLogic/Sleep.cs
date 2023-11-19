using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;

namespace GameModule.Logic.TasksLogic
{
    public class Sleep : ITask
    {
        const int MINIMUM_FOOD_LVL_TO_START_SLEEP = 10;
        const int MINIMUM_THERMAL_LVL_TO_START_SLEEP = 10;

        INotification? ITask.Start(HumanTaskOrder taskOrder, 
            Tribe tribe, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            taskOrder = null; //auto task

            var humansWithConditions = BasicDataSelector
                .SelectHumansWithCondition(tribe)
                .Where(x => x.FoodLevelPercentage > MINIMUM_FOOD_LVL_TO_START_SLEEP)
                .Where(x => x.ThermalLevelPercentage > MINIMUM_THERMAL_LVL_TO_START_SLEEP);

            var human = RandomCalculator.GetRandomItemFromList(humansWithConditions);

            if (human == null)
                return default;

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);
            if (!shouldAssign)
                return default;

            var entity = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                TribeId = tribe.Id,
                Type = ETaskType.Sleep,
                To = currentTimeInLoop.AddMinutes(DayNightService.NIGHT_DURATION_MINUTES),
                Localization = null,
                IsCompleted = false,
            };
            tribe.HumanTasks.Add(entity);

            return new NotificationDto(human.Id,
                human.Name,
                currentTimeInLoop,
                ENotificationType.SleepStart,
                entity.To.ToString());
        }
        
        INotification? ITask.End(HumanTask taskToEnd, 
            Tribe tribe, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            tribe.HumanTasks.Remove(taskToEnd);

            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                currentTimeInLoop,
                ENotificationType.SleepEnd,
                null);
        }

    }
}
