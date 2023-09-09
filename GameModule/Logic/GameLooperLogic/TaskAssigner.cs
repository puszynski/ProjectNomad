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
            List<MapTile> mapTiles, 
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
            List<MapTile> mapTiles,
            List<INotification> notifications)
        {

            if (!tribe.HumanUnitTaskOrders.Where(x => !x.IsInProgress).Any())
                return;

            var humanIDsWithTaskAssigned = tribe.HumanUnitTasks.Select(x => x.HumanUnitId);
            var humansWithConditionToStartNewTask = tribe
                .HumanUnits
                .Where(x => !humanIDsWithTaskAssigned.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage > 20);//starvation, too week to work..


            foreach (var human in humansWithConditionToStartNewTask)
            {
                var taskOrderToAssign = tribe.HumanUnitTaskOrders.Where(x => !x.IsInProgress).OrderBy(x => x.Added).FirstOrDefault();

                if (taskOrderToAssign == null)
                    return;

                var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.ProbabilityToAssignToTaskOrder); //todo base on some parameters like foodLevel, morals..

                if (!shouldAssign)
                    continue;

                var destinyMapTile = mapTiles.Single(x => x.Id == taskOrderToAssign.MapTileId);

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

            var timeToEndTask = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, human.FoodLevelPercentage) + TimeSpan.FromMinutes(GameSETTINGS.MinutesToGatherFood);
            return _dateTimeProvider.UtcNow().Add(timeToEndTask);
        }
    }
}
