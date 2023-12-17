using GameModule.DtoModels;
using GameModule.Entities;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;
using GameModule.Logic.GameLooperLogic.TasksLogic.Helpers;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    public class Sleep : IAutoTaskStart, ITaskEnd
    {
        const int MINIMUM_FOOD_LVL_TO_START_SLEEP = 10;
        const int MINIMUM_THERMAL_LVL_TO_START_SLEEP = 10;

        INotification? IAutoTaskStart.Start(
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var humansWithConditions = BasicDataSelector
                .SelectHumansWithCondition(tribe)
                .Where(x => x.FoodLevelPercentage > MINIMUM_FOOD_LVL_TO_START_SLEEP)
                .Where(x => x.ThermalLevelPercentage > MINIMUM_THERMAL_LVL_TO_START_SLEEP);

            var human = RandomCalculator.GetRandomItemFromList(humansWithConditions);

            if (human == null)
                return default;

            var entity = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Human = human,
                TribeId = tribe.Id,
                Type = ETaskType.Sleep,
                To = currentTimeInLoop.AddMinutes(DayNightService.NIGHT_DURATION_MINUTES),
                Localization = null
            };
            tribe.HumanTasks.Add(entity);

            return new NotificationDto(human.Id,
                human.Name,
                currentTimeInLoop,
                ENotificationType.SleepStart,
                entity.To.ToString());
        }

        INotification? ITaskEnd.End(HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                currentTimeInLoop,
                ENotificationType.SleepEnd,
                null);
        }

    }
}
