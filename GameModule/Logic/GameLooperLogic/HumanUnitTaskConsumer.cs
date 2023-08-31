using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumanUnitTaskConsumer
    {
        IEnumerable<INotification> Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> allTasksToConsume,
            List<MapTile> mapTilesToConsumeTasks,
            DateTime currentTimeInLoop);
    }

    internal class HumanUnitTaskConsumer : IHumanUnitTaskConsumer
    {
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        public HumanUnitTaskConsumer(IHumanUnitTaskRepository humanUnitTaskRepository)
        {
            _humanUnitTaskRepository = humanUnitTaskRepository;
        }

        IEnumerable<INotification> IHumanUnitTaskConsumer.Execute(List<HumanUnit> humanUnits, 
            Tribe tribe,
            List<HumanUnitTask> allTasksToConsume,
            List<MapTile> mapTilesToConsumeTasks,
            DateTime currentTimeInLoop)
        {
            var humanUnitIds = humanUnits
                .Select(x => x.Id)
                .ToList();

            var tasksToConsume = allTasksToConsume
                .Where(x => humanUnitIds.Contains(x.HumanUnitId))
                .Where(x => x.To <= currentTimeInLoop)
                .ToList();

            var notificationToSendToClient = new List<INotification>();

            foreach (var task in tasksToConsume) 
            {
                var notification = ConsumeTask(task, 
                    humanUnits.Single(x => x.Id == task.HumanUnitId), 
                    tribe,
                    mapTilesToConsumeTasks,
                    currentTimeInLoop);

                if (notification != null)
                    notificationToSendToClient.Add(notification);

                allTasksToConsume.Remove(task);
                _humanUnitTaskRepository.Remove(task);
            }

            return notificationToSendToClient;
        }

        INotification? ConsumeTask(HumanUnitTask humanUnitTask,
            HumanUnit humanUnit,
            Tribe tribe,
            List<MapTile> mapTilesToConsumeTasks,
            DateTime currentTimeInLoop)
        {
            switch (humanUnitTask.Type)
            {
                case EHumanUnitTaskType.GatheringFood:
                    const int MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK = 5;

                    var mapTile = mapTilesToConsumeTasks.Single(x => x.Id == humanUnitTask.MapTileId);
                    var mapTileFoodPoints = mapTile.Food.ActualPoints;

                    var foodGatheringCoefficient = (double)humanUnit.FoodLevelPercentage / 100 * 2;
                    var foodPoints = foodGatheringCoefficient >= 1 
                        ? MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK
                        : foodGatheringCoefficient * MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK;

                    var gatheredFoodPoints = mapTileFoodPoints > foodPoints 
                        ? foodPoints 
                        : mapTileFoodPoints;

                    tribe.Resources.FreshFood += (int)gatheredFoodPoints;
                    return new NotificationDto(humanUnit.Id, humanUnit.Name, currentTimeInLoop, ENotificationType.FoodGatheringEnded, gatheredFoodPoints.ToString());

                case EHumanUnitTaskType.ConsumeFood:
                    //note: consumption of the food applies when task is created
                    return new NotificationDto(humanUnit.Id, humanUnit.Name, currentTimeInLoop, ENotificationType.FoodConsumptionEnded, null);

                default: throw new NotImplementedException();
            }
        }
    }
}
