using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumanUnitTaskConsumer
    {
        void Execute(IEnumerable<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> allTasksToConsume,
            List<MapTile> mapTilesToConsumeTasks);
    }

    internal class HumanUnitTaskConsumer : IHumanUnitTaskConsumer
    {
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        public HumanUnitTaskConsumer(IDateTimeProvider dateTimeProvider, 
            IHumanUnitTaskRepository humanUnitTaskRepository)
        {
            _dateTimeProvider = dateTimeProvider;
            _humanUnitTaskRepository = humanUnitTaskRepository;
        }

        void IHumanUnitTaskConsumer.Execute(IEnumerable<HumanUnit> humanUnits, 
            Tribe tribe,
            List<HumanUnitTask> allTasksToConsume,
            List<MapTile> mapTilesToConsumeTasks)
        {
            var humanUnitIds = humanUnits
                .Select(x => x.Id)
                .ToList();

            var tasksToConsume = allTasksToConsume
                .Where(x => humanUnitIds.Contains(x.HumanUnitId))
                .Where(x => x.To <= _dateTimeProvider.UtcNow()) //note - gameLOOPER is triggered from client once per minute.. 
                .ToList();

            foreach (var task in tasksToConsume) 
            {
                var mapTile = mapTilesToConsumeTasks.Single(x => x.Id == task.MapTileId);
                
                ConsumeTask(task, 
                    humanUnits.Single(x => x.Id == task.HumanUnitId), 
                    tribe,
                    mapTile.Food.ActualPoints);

                allTasksToConsume.Remove(task);//todo test it
                _humanUnitTaskRepository.Remove(task);
            }
        }

        void ConsumeTask(HumanUnitTask humanUnitTask,
            HumanUnit humanUnit,
            Tribe tribe,
            int mapTileFoodPoints) //todo UT
        {
            switch (humanUnitTask.Type)
            {
                case EHumanUnitTaskType.GatheringFood:
                    const int MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK = 5;

                    var foodGatheringCoefficient = (double)humanUnit.FoodLevelPercentage / 100 * 2;
                    var foodPoints = foodGatheringCoefficient >= 1 
                        ? MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK
                        : foodGatheringCoefficient * MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK;

                    var gatheredFoodPoints = mapTileFoodPoints > foodPoints 
                        ? foodPoints 
                        : mapTileFoodPoints;

                    tribe.Resources.FreshFood += (int)gatheredFoodPoints;
                    break;

                case EHumanUnitTaskType.ConsumeFood:
                    humanUnit.FoodLevelPercentage += 20;
                    break;
            }
        }
    }
}
