using GameModule.DtoModels;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

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

            var humanIDsWithTaskAssigned = tribe.HumanUnitTasks.Select(x => x.HumanUnitId);
            var humansWithConditionToStartNewTask = tribe
                .HumanUnits
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id)) //only 1 task per human unit
                .Where(x => x.FoodLevelPercentage > 20); //too week to work..

            //TODO!!! SPLIT for each type of tasks.. now y have here only food gathering started <= BUILD FACTORY
            foreach (var human in humansWithConditionToStartNewTask)
            {
                var taskOrderToAssign = tribe.HumanUnitTaskOrders.Where(x => !x.IsInProgress).OrderBy(x => x.Added).FirstOrDefault();

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
                    To = CalculateTimeToEndTask(destinyMapTile, tribe, human),
                    TribeId = tribe.Id,
                    Type = taskOrderToAssign.Type,
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
            }
        }

        DateTime CalculateTimeToEndTask(MapTile destinyMapTile, Tribe tribe, HumanUnit human)
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
