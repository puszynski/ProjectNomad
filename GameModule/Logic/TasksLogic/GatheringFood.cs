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
        INotification ITask.Start(HumanTaskOrder taskOrder, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var human = BasicDataSelector.SelectFirstHumanWithCondition(tribe);
            if (human == null)
                return default;

            var taskOrderToAssign = tribe.HumanTaskOrders
                .Where(x => x.Type == ETaskType.GatheringFood)
                .Where(x => !x.HumanTaskId.HasValue)
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

            var taskToAdd = new Entities.HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Localization = destinyMapTile.Localization,
                To = CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = ETaskType.GatheringFood,
            };

            //temp check
            if (taskToAdd.Localization == null)
                throw new NullReferenceException("Localization cant be null!");

            taskOrderToAssign.HumanTask = taskToAdd;
            destinyMapTile.Food.ActualPoints -= GameSETTINGS.Food.MapTileFoodGathered;
            tribe.HumanTasks.Add(taskToAdd);

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

        INotification ITask.End(HumanTask taskToEnd, Tribe tribe, DateTime currentTimeInLoop, IEnumerable<MapTile> mapTiles)
        {
            var mapTile = mapTiles.SingleOrDefault(x => x.Localization.Equals(taskToEnd.Localization));//wkradł się task który nie ma lokalizacji.. jak? taskToEnd nie ma lokalizacji
            if (mapTile == null)//temp - after fir change to Single()
                throw new NullReferenceException("MapTile can not be a null. GatheringFood.End()");

            var mapTileFoodPoints = mapTile.Food.ActualPoints;

            var human = tribe.Humans.Single(x => x.Id == taskToEnd.HumanId);

            var foodGatheringCoefficient = (double)human.FoodLevelPercentage / 100 * 2;
            var foodPoints = foodGatheringCoefficient >= 1
                ? GameSETTINGS.Food.MapTileFoodGathered
                : foodGatheringCoefficient * GameSETTINGS.Food.MapTileFoodGathered;

            var gatheredFoodPoints = mapTileFoodPoints > foodPoints
                ? foodPoints
                : mapTileFoodPoints;

            tribe.Resources.FreshFood += (int)gatheredFoodPoints;            

            return new NotificationDto(taskToEnd.HumanId,
                human.Name,
                currentTimeInLoop, 
                ENotificationType.FoodGatheringEnded, 
                gatheredFoodPoints.ToString());
        }

    }
}
