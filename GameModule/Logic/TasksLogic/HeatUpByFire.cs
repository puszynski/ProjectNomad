using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class HeatUpByFire : ITask //todo implement
    {
        //ThermalLevelPercentage
        private const int HUMAN_THERMAL_PERCENTAGE_TO_START = 30;

        INotification ITask.Start(HumanTaskOrder taskOrder, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var humansWithConditions = BasicDataSelector
                .SelectHumansWithCondition(tribe)
                .Where(x => x.ThermalLevelPercentage <= HUMAN_THERMAL_PERCENTAGE_TO_START);

            var human = RandomCalculator.GetRandomItemFromList(humansWithConditions);

            if (human == null)
                return default;

            if (!tribe.TribeStructures.Any(x => x.Type == ETribeStructureType.Firecamp && x.PowerAndDurability > 1))
                return default;

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);
            if (!shouldAssign)
                return default;

            var entity = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                TribeId = tribe.Id,
                Type = ETaskType.HeatUpHumanByFireEnded,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Fire.TimeToHeatUpHumanByFire),
                Localization = null,
                IsCompleted = false,
            };

            tribe.HumanTasks.Add(entity);
            human.ThermalLevelPercentage += 20; //todo const

            return new NotificationDto(human.Id,
                human.Name,
                currentTimeInLoop,
                ENotificationType.FoodConsumptionStarted,
                entity.To.ToString());
        }

        INotification ITask.End(HumanTask taskToEnd, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            tribe.HumanTasks.Remove(taskToEnd);

            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                currentTimeInLoop,
                ENotificationType.HeatUpHumanByFireEnded,
                null);
        }
    }
}
