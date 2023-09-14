using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using System.Diagnostics;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ITaskAssigner
    {
        Task Execute(Tribe tribe,
            ICollection<MapTile> mapTiles, 
            List<INotification> notifications);
    }

    internal class TaskAssigner : ITaskAssigner
    {
        readonly IDateTimeProvider _dateTimeProvider;
        public TaskAssigner(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        async Task ITaskAssigner.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications)
        {

            if (!tribe.HumanUnitTaskOrders.Where(x => !x.IsInProgress).Any())
                return;

            //todo factory
            GatheringFoodTasksAssign(tribe, mapTiles, notifications);
            TribeRelocationTasksAssign(tribe, mapTiles);

        }

        void TribeRelocationTasksAssign(Tribe tribe, ICollection<MapTile> mapTiles)
        {
            //1 w8 for some time (player can cancell if missclicked etc)
            //2 validate resources etc

            var relocationTaskOrder = tribe.HumanUnitTaskOrders
                .Where(x => x.Type == EHumanUnitTaskType.TribeRelocation)
                .SingleOrDefault();

            if (relocationTaskOrder == null)
                return;

            var mapTile = mapTiles.Single(x => x.Id == relocationTaskOrder.MapTileId);

            var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                        tribe.Localization.Y,
                        mapTile.Localization.X,
                        mapTile.Localization.Y);

            var relocation = new TribeRelocation
            {
                From = _dateTimeProvider.UtcNow(),
                To = _dateTimeProvider.UtcNow().AddMinutes(distance * GameSETTINGS.Moving.MinutesToTravelOneTileWhileTribeIsRelocating),
                Start = new Localization { X = tribe.Localization.X, Y = tribe.Localization.Y },
                Destiny = new Localization { X = mapTile.Localization.X, Y = mapTile.Localization.Y }, 
            };

            tribe.TribeRelocation = relocation;
            tribe.HumanUnitTaskOrders.Remove(relocationTaskOrder);
        }

        async Task GatheringFoodTasksAssign(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications)
        {
            var humanIDsWithTaskAssigned = tribe.HumanUnitTasks.Select(x => x.HumanUnitId);
            var humansWithConditionToStartNewTask = tribe
                .HumanUnits
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id)) //only 1 task per human unit
                .Where(x => x.FoodLevelPercentage > 20); //too week to work..

            //TODO!!! SPLIT for each type of tasks.. now y have here only food gathering started <= BUILD FACTORY
            foreach (var human in humansWithConditionToStartNewTask)
            {
                var taskOrderToAssign = tribe.HumanUnitTaskOrders
                    .Where(x => x.Type == EHumanUnitTaskType.GatheringFood)
                    .Where(x => !x.IsInProgress)
                    .OrderBy(x => x.Added)
                    .FirstOrDefault();

                if (taskOrderToAssign == null)
                    return;

                var destinyMapTile = mapTiles.Single(x => x.Id == taskOrderToAssign.MapTileId);

                if (destinyMapTile.Food.ActualPoints < GameSETTINGS.Food.MapTileFoodGathered)
                    return;

                var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond); //todo base on some parameters like foodLevel, morals..

                if (!shouldAssign)
                    continue;


                var taskToAdd = new HumanUnitTask
                {
                    From = _dateTimeProvider.UtcNow(),
                    HumanUnitId = human.Id,
                    MapTileId = taskOrderToAssign.MapTileId,
                    To = CalculateTimeToEndTask(),
                    TribeId = tribe.Id,
                    Type = EHumanUnitTaskType.GatheringFood,
                };

                taskOrderToAssign.IsInProgress = true;
                destinyMapTile.Food.ActualPoints -= GameSETTINGS.Food.MapTileFoodGathered;

                var notification = new NotificationDto(human.Id,
                    human.Name,
                    taskToAdd.From,
                    ENotificationType.FoodGatheringStarted,
                    taskToAdd.To.ToString());
                notifications.Add(notification);

                tribe.HumanUnitTasks.Add(taskToAdd);

                //nested
                DateTime CalculateTimeToEndTask()
                {
                    var distance = MapTileDistanceCalculator.Execute(tribe.Localization.X,
                        tribe.Localization.Y,
                        destinyMapTile.Localization.X,
                        destinyMapTile.Localization.Y);

                    var timeToEndTask = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, human.FoodLevelPercentage) + TimeSpan.FromMinutes(GameSETTINGS.Food.MinutesToGatherFood);
                    return _dateTimeProvider.UtcNow().Add(timeToEndTask);
                }
            }
        }
    }
}
