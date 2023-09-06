using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ITaskAssigner
    {
        Task<IEnumerable<INotification>> Execute(List<HumanUnitTaskOrder> humanUnitTaskOrders,
            List<HumanUnitTask> humanUnitTasks,
            List<HumanUnit> humanUnits,
            Tribe tribe,
            List<MapTile> mapTiles);
    }

    internal class TaskAssigner : ITaskAssigner
    {
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        public TaskAssigner(IHumanUnitTaskRepository humanUnitTaskRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _humanUnitTaskRepository = humanUnitTaskRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        async Task<IEnumerable<INotification>> ITaskAssigner.Execute(List<HumanUnitTaskOrder> humanUnitTaskOrders,
            List<HumanUnitTask> humanUnitTasks,
            List<HumanUnit> humanUnits,
            Tribe tribe,
            List<MapTile> mapTiles)
        {
            var notifications = new List<INotification>();

            if (!humanUnitTaskOrders.Where(x => !x.IsInProgress).Any())
                return notifications;

            var humanIDsWithTaskAssigned = humanUnitTasks.Select(x => x.HumanUnitId);
            var humansWithNoTasks = humanUnits.Where(x => !humanIDsWithTaskAssigned.Contains(x.Id));


            foreach (var human in humansWithNoTasks)
            {
                var taskOrderToAssign = humanUnitTaskOrders.Where(x => !x.IsInProgress).OrderBy(x => x.Added).FirstOrDefault();

                if (taskOrderToAssign == null)
                    return notifications;

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

                //await _humanUnitTaskRepository.SaveChangesAsync(); ze niby pomoglo, jak nie wiesz o co common to wywal
                await _humanUnitTaskRepository.AddAsync(taskToAdd);
                await _humanUnitTaskRepository.SaveChangesAsync(); 
                humanUnitTasks.Add(taskToAdd);
            }

            return notifications;
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
