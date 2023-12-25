using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    internal class HeatUpByFire : IAutoTaskStart, ITaskEnd
    {
        const int HUMAN_THERMAL_PERCENTAGE_TO_START = 30;
        const int HEAT_UP_POINTS = 40;

        INotification IAutoTaskStart.Start(
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var humansWithConditions = BasicDataSelector
                .SelectHumansWithNoTasksAssigned(tribe, notInCriticalCondition: false)
                .Where(x => x.ThermalLevelPercentage <= HUMAN_THERMAL_PERCENTAGE_TO_START);

            var human = RandomCalculator.GetRandomItemFromList(humansWithConditions);

            if (human == null)
                return default;

            if (!tribe.TribeStructures.Any(x => x.Type == ETribeStructureType.Firecamp && x.PowerAndDurability > 1))
                return default;

            var task = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Human = human,
                TribeId = tribe.Id,
                Type = ETaskType.HeatUpHumanByFireEnded,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Fire.TimeToHeatUpHumanByFire),
                Localization = tribe.Localization
            };

            tribe.HumanTasks.Add(task);
            human.HumanTask = task;
            human.ThermalLevelPercentage += HEAT_UP_POINTS;

            return new NotificationDto(human.Id,
                human.Name,
                currentTimeInLoop,
                ENotificationType.FoodConsumptionStarted,
                task.To.ToString());
        }

        INotification ITaskEnd.End(
            HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                currentTimeInLoop,
                ENotificationType.HeatUpHumanByFireEnded,
                null);
        }
    }
}
