using GameModule.Entities;
using GameModule.Repositories;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ITaskAssigner
    {
        Task Execute(List<HumanUnitTaskOrder> humanUnitTaskOrders,
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

        async Task ITaskAssigner.Execute(List<HumanUnitTaskOrder> humanUnitTaskOrders,
            List<HumanUnitTask> humanUnitTasks,
            List<HumanUnit> humanUnits,
            Tribe tribe,
            List<MapTile> mapTiles)
        {
            var humanIDsWithTaskAssigned = humanUnitTasks.Select(x => x.HumanUnitId);
            var humansWithNoTasks = humanUnits.Where(x => !humanIDsWithTaskAssigned.Contains(x.Id));

            //OPTION 1
            foreach (var human in humansWithNoTasks)
            {
                var taskOrderToAssign = humanUnitTaskOrders.Where(x => !x.IsInProgress).OrderBy(x => x.Added).FirstOrDefault();

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

                await _humanUnitTaskRepository.AddAsync(taskToAdd);
                await _humanUnitTaskRepository.SaveChangesAsync();
                humanUnitTasks.Add(taskToAdd);
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
