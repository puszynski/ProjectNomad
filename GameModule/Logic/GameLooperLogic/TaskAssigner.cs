using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
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
        readonly ITribeRelocationService _relocationService;
        public TaskAssigner(IDateTimeProvider dateTimeProvider, 
            ITribeRelocationService relocationService)
        {
            _dateTimeProvider = dateTimeProvider;
            _relocationService = relocationService;
        }

        async Task ITaskAssigner.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            List<INotification> notifications)
        {

            if (!tribe.HumanUnitTaskOrders.Where(x => !x.IsInProgress).Any())
                return;

            //todo factory
            GatheringFoodTasksAssign(tribe, mapTiles, notifications);
            TribeRelocationTasksAssign(tribe);

        }

        void TribeRelocationTasksAssign(Tribe tribe)
        {
            var taskOrderToAssign = tribe.HumanUnitTaskOrders
                    .Where(x => x.Type == EHumanUnitTaskType.TribeRelocation)
                    .Where(x => !x.IsInProgress)
                    .SingleOrDefault();

            if (taskOrderToAssign == null)
                return;

            if (taskOrderToAssign.Added > _dateTimeProvider.UtcNow().AddMinutes(-1)) //time for player to cancell order
                return;

            //2 validate resources etc
            if (!_relocationService.IsValidToStartRelocationProcess()) //always false - when ready, change..
                return;

            _relocationService.StartRelocationProcess(tribe);

            
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

                var destinyMapTile = mapTiles.Single(x => x.Localization.Equals(taskOrderToAssign.Localization));

                if (destinyMapTile.Food.ActualPoints < GameSETTINGS.Food.MapTileFoodGathered)
                    return;

                var shouldAssign = RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond); //todo base on some parameters like foodLevel, morals..

                if (!shouldAssign)
                    continue;


                var taskToAdd = new HumanUnitTask
                {
                    From = _dateTimeProvider.UtcNow(),
                    HumanUnitId = human.Id,
                    Localization = taskOrderToAssign.Localization,
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
