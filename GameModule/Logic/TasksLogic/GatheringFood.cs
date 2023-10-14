using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class GatheringFood : ITask
    {
        INotification ITask.Start(HumanUnitTaskOrder taskOrder, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var human = BasicDataSelector.SelectFirstHumanWithCondition(tribe);
            if (human == null)
                return default;

            var taskOrderToAssign = tribe.HumanUnitTaskOrders
                .Where(x => x.Type == EHumanUnitTaskType.GatheringFood)
                .Where(x => !x.IsInProgress)
                .OrderBy(x => x.Added)
                .FirstOrDefault();

            if (taskOrderToAssign == null)
                return default;

            var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskOrderToAssign.Localization));

            if (destinyMapTile.Food.ActualPoints < GameSETTINGS.Food.MapTileFoodGathered)
                return default;

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);

            if (!shouldAssign)
                return default;

            var taskToAdd = new HumanUnitTask
            {
                From = currentTimeInLoop,
                HumanUnitId = human.Id,
                Localization = taskOrderToAssign.Localization,
                To = CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = EHumanUnitTaskType.GatheringFood,
            };
            taskOrderToAssign.IsInProgress = true;
            destinyMapTile.Food.ActualPoints -= GameSETTINGS.Food.MapTileFoodGathered;
            tribe.HumanUnitTasks.Add(taskToAdd);

            return new NotificationDto(human.Id,
                human.Name,
                taskToAdd.From,
                ENotificationType.FoodGatheringStarted,
                taskToAdd.To.ToString());


            DateTime CalculateTimeToEndTask()
            {
                var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                    tribe.Localization.Y,
                    destinyMapTile.Localization.X,
                    destinyMapTile.Localization.Y);

                var timeToEndTask = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, human.FoodLevelPercentage) + TimeSpan.FromMinutes(GameSETTINGS.Food.MinutesToGatherFood);
                return currentTimeInLoop.Add(timeToEndTask);
            }

        }

        INotification ITask.End(HumanUnitTask taskToEnd, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var mapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));
            var mapTileFoodPoints = mapTile.Food.ActualPoints;

            var human = tribe.HumanUnits.Single(x => x.Id == taskToEnd.HumanUnitId);

            var foodGatheringCoefficient = (double)human.FoodLevelPercentage / 100 * 2;
            var foodPoints = foodGatheringCoefficient >= 1
                ? GameSETTINGS.Food.MapTileFoodGathered
                : foodGatheringCoefficient * GameSETTINGS.Food.MapTileFoodGathered;

            var gatheredFoodPoints = mapTileFoodPoints > foodPoints
                ? foodPoints
                : mapTileFoodPoints;

            tribe.Resources.FreshFood += (int)gatheredFoodPoints;            

            return new NotificationDto(taskToEnd.HumanUnitId,
                human.Name,
                currentTimeInLoop, 
                ENotificationType.FoodGatheringEnded, 
                gatheredFoodPoints.ToString());
        }

    }
}
