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
        INotification? ITask.Start(HumanTaskOrder taskOrder, 
            Tribe tribe, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            if (tribe.HumanTaskOrders == null)
                return default;

            var human = BasicDataSelector.SelectFirstHumanWithCondition(tribe);
            if (human == null)
                return default;

            if (taskOrder == null)
                return default;

            var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskOrder.Localization));

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);

            if (!shouldAssign)
                return default;

            var taskToAdd = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Localization = destinyMapTile.Localization,
                To = CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = ETaskType.GatheringFood,
                IsCompleted = false,
            };

            taskOrder.HumanTask = taskToAdd;

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

        INotification? ITask.End(HumanTask taskToEnd, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            if (tribe.Humans == null)
                return default;

            if (taskToEnd.Localization == null)
                throw new Exception("Localization in HumanTask can not be null");

            var mapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));

            var mapTileFoodPoints = mapTile.Food.ActualPoints;

            var human = tribe.Humans.Single(x => x.Id == taskToEnd.HumanId);

            var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));

            if (destinyMapTile.Food.ActualPoints < GameSETTINGS.Food.MapTileFoodGathered)
            {
                taskToEnd.IsCompleted = true;

                return new NotificationDto(taskToEnd.HumanId,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.NoFoodFounded,
                    CustomValue: null);
            }
            else
            {
                var foodGatheringCoefficient = (double)human.FoodLevelPercentage / 100 * 2;
                var foodPoints = foodGatheringCoefficient >= 1
                    ? GameSETTINGS.Food.MapTileFoodGathered
                    : foodGatheringCoefficient * GameSETTINGS.Food.MapTileFoodGathered;

                var gatheredFoodPoints = mapTileFoodPoints > foodPoints
                    ? foodPoints
                    : mapTileFoodPoints;

                destinyMapTile.Food.ActualPoints -= GameSETTINGS.Food.MapTileFoodGathered;
                tribe.Resources.FreshFood += (int)gatheredFoodPoints;
                taskToEnd.IsCompleted = true;

                return new NotificationDto(taskToEnd.HumanId,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.FoodGatheringEnded,
                    gatheredFoodPoints.ToString());
            }
        }

    }
}
