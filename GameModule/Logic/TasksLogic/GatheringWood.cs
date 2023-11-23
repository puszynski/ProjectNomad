using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Logic;

namespace GameModule.Logic.TasksLogic
{
    internal class GatheringWood : ITask
    {
        INotification ITask.Start(HumanTaskOrder taskOrder,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var human = BasicDataSelector.SelectFirstHumanWithCondition(tribe);

            if (taskOrder == null || human == null)
                return default;

            var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskOrder.Localization));

            if (destinyMapTile.Wood.ActualPoints < GameSETTINGS.Wood.WoodAmountGatheredFromMap)
                return default;

            var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                    tribe.Localization.Y,
                    destinyMapTile.Localization.X,
                    destinyMapTile.Localization.Y);

            if (taskOrder.HumanTask != null)
                return default;

            var taskToAdd = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = human.Id,
                Localization = taskOrder.Localization,
                To = currentTimeInLoop.Add(TaskDurationCalculator.WoodGathering(distance)), //CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = ETaskType.GatheringWood,
                IsCompleted = false,
            };

            taskOrder.HumanTask = taskToAdd;
            //tribe.HumanTasks.Add(taskToAdd); //chyba niepotrzebne?
            
            return new NotificationDto(human.Id,
               human.Name,
               taskToAdd.From,
               ENotificationType.WoodGatheringStarted,
               taskToAdd.To.ToString());
        }


        INotification ITask.End(HumanTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var mapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));
            var mapTileWoodPoints = mapTile.Wood.ActualPoints;

            var human = tribe.Humans.Single(x => x.Id ==  taskToEnd.HumanId);


            var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));

            if (destinyMapTile.Wood.ActualPoints < GameSETTINGS.Wood.WoodAmountGatheredFromMap)
            {
                taskToEnd.IsCompleted = true;

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


                destinyMapTile.Wood.ActualPoints -= GameSETTINGS.Wood.WoodAmountGatheredFromMap;
                tribe.Resources.Wood += (int)gatheredWoodPoints;
                taskToEnd.IsCompleted = true;

                return new NotificationDto(taskToEnd.HumanId,
                    human.Name,
                    currentTimeInLoop,
                    ENotificationType.WoodGatheringEnded,
                    gatheredWoodPoints.ToString());
            }
        }
    }
}
