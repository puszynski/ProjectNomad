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
        INotification ITask.Start(HumanUnitTaskOrder taskOrder,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var human = BasicDataSelector.SelectFirstHumanWithCondition(tribe);

            if (taskOrder == null)
                return default;

            var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskOrder.Localization));

            if (destinyMapTile.Wood.ActualPoints < GameSETTINGS.Wood.WoodAmountGatheredFromMap)
                return default;

            var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond);

            if (!shouldAssign)
                return default;

            var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                    tribe.Localization.Y,
                    destinyMapTile.Localization.X,
                    destinyMapTile.Localization.Y);

            var taskToAdd = new HumanUnitTask
            {
                From = currentTimeInLoop,
                HumanUnitId = human.Id,
                Localization = taskOrder.Localization,
                To = currentTimeInLoop.Add(TaskDurationCalculator.WoodGathering(distance)), //CalculateTimeToEndTask(),
                TribeId = tribe.Id,
                Type = EHumanUnitTaskType.GatheringWood,
            };

            taskOrder.IsInProgress = true;
            destinyMapTile.Wood.ActualPoints -= GameSETTINGS.Wood.WoodAmountGatheredFromMap;
            tribe.HumanUnitTasks.Add(taskToAdd);
            
            return new NotificationDto(human.Id,
               human.Name,
               taskToAdd.From,
               ENotificationType.WoodGatheringStarted,
               taskToAdd.To.ToString());
        }


        INotification ITask.End(HumanUnitTask taskToEnd,
            Tribe tribe,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            var mapTile = mapTiles.Single(x => x.Localization.Equals(taskToEnd.Localization));
            var mapTileWoodPoints = mapTile.Wood.ActualPoints;

            var woodGatheringCoefficient = (double)taskToEnd.HumanUnit.FoodLevelPercentage / 100 * 2;
            var woodPoints = woodGatheringCoefficient >= 1
                ? GameSETTINGS.Wood.WoodAmountGatheredFromMap
                : woodGatheringCoefficient * GameSETTINGS.Wood.WoodAmountGatheredFromMap;

            var gatheredWoodPoints = mapTileWoodPoints > woodPoints
                ? woodPoints
                : mapTileWoodPoints;

            tribe.Resources.Wood += (int)gatheredWoodPoints;

            return new NotificationDto(taskToEnd.HumanUnitId, 
                taskToEnd.HumanUnit.Name, 
                currentTimeInLoop, 
                ENotificationType.WoodGatheringEnded, 
                gatheredWoodPoints.ToString());
        }
    }
}
