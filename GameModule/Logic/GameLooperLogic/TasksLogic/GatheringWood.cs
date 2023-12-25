using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;

namespace GameModule.Logic.GameLooperLogic.TasksLogic
{
    internal class GatheringWood : ITaskFromJobStart, ITaskEnd
    {
        readonly MapTileFetcher _mapTileFetcher;

        public GatheringWood(MapTileFetcher mapTileFetcher)
        {
            _mapTileFetcher = mapTileFetcher;
        }

        INotification ITaskFromJobStart.Start(
            Human human,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var destinyMapTile = _mapTileFetcher.AssignMapTileToJobAndFetchMissingMapTiles(
                mapTiles,
                ETaskType.GatheringWood,
                tribe.Localization);

            if (destinyMapTile == null)
                return new NotificationDto(
                    human.Id,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.NoWoodInCampArea,
                    CustomValue: null);

            if (destinyMapTile.Wood.ActualPoints < GameSETTINGS.Wood.WoodAmountGatheredFromMap)
                return default;

            var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                    tribe.Localization.Y,
                    destinyMapTile.Localization.X,
                    destinyMapTile.Localization.Y);


            var taskToAdd = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Human = human,
                Localization = destinyMapTile.Localization,
                To = currentTimeInLoop.Add(TaskDurationCalculator.WoodGathering(distance)), //CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = ETaskType.GatheringWood
            };

            tribe.HumanTasks.Add(taskToAdd);
            human.HumanTask = taskToAdd;

            return new NotificationDto(
                human.Id,
                human.Name,
                currentTimeInLoop,
                ENotificationType.WoodGatheringStarted,
                CustomValue: taskToAdd.To.ToString());
        }


        INotification ITaskEnd.End(HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            if (taskToEnd.Localization == null)
                return default; //todo LOG + TRY FIND WHY IT HAPPEND..
                //throw new ApplicationException();

            var mapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));
            var mapTileWoodPoints = mapTile.Wood.ActualPoints;

            Human human = tribe.Humans.Single(x => x.Id == taskToEnd.HumanId);
            

            if (mapTile.Wood.ActualPoints < GameSETTINGS.Wood.WoodAmountGatheredFromMap)
            {
                return new NotificationDto(taskToEnd.HumanId,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.NoWoodFounded,
                    null);
            }
            else
            {
                var woodGatheringCoefficient = (double)human.FoodLevelPercentage / 100 * 2;
                var woodPoints = woodGatheringCoefficient >= 1
                    ? GameSETTINGS.Wood.WoodAmountGatheredFromMap
                    : woodGatheringCoefficient * GameSETTINGS.Wood.WoodAmountGatheredFromMap;

                var gatheredWoodPoints = mapTileWoodPoints > woodPoints
                    ? woodPoints
                    : mapTileWoodPoints;


                mapTile.Wood.ActualPoints -= GameSETTINGS.Wood.WoodAmountGatheredFromMap;
                tribe.Resources.Wood += (int)gatheredWoodPoints;

                return new NotificationDto(taskToEnd.HumanId,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.WoodGatheringEnded,
                    gatheredWoodPoints.ToString());
            }
        }
    }
}
