using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    internal class GatheringFood : ITaskFromJobStart, ITaskEnd
    {
        readonly MapTileFetcher _mapTileFetcher;
        public GatheringFood(MapTileFetcher mapTileFetcher)
        {
            _mapTileFetcher = mapTileFetcher;
        }

        (HumanTask?, INotification) ITaskFromJobStart.Start(
            Human human,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var destinyMapTile = _mapTileFetcher.AssignMapTileToJobAndFetchMissingMapTiles(
                mapTiles,
                ETaskType.GatheringFood,
                tribe.Localization);

            var task = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Human = human,
                Localization = destinyMapTile.Localization,
                To = CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = ETaskType.GatheringFood
            };

            var notification = new NotificationDto(human.Id,
                human.Name,
                task.From,
                ENotificationType.FoodGatheringStarted,
                task.To.ToString());

            return (task, notification);

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

        INotification? ITaskEnd.End(
            HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            if (tribe.Humans == null)
                return default;

            if (taskToEnd.Localization == null)
                throw new Exception("(!) Localization in HumanTask can not be null");

            var mapTile = mapTiles
                .Single(x => x.Localization.Equals(taskToEnd.Localization));

            var mapTileFoodPoints = mapTile.Food.ActualPoints;

            var human = taskToEnd.Human;

            if (mapTile.Food.ActualPoints < GameSETTINGS.Food.MapTileFoodGathered)
            {
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

                mapTile.Food.ActualPoints -= GameSETTINGS.Food.MapTileFoodGathered;
                tribe.Resources.FreshFood += (int)gatheredFoodPoints;

                return new NotificationDto(taskToEnd.HumanId,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.FoodGatheringEnded,
                    gatheredFoodPoints.ToString());
            }
        }

    }
}
