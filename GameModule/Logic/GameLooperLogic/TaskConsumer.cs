using GameModule.DtoModels;
using GameModule.Entities;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ITaskConsumer
    {
        void Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop,
            List<INotification> notifications);
    }

    internal class TaskConsumer : ITaskConsumer
    {
        void ITaskConsumer.Execute(Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop,
            List<INotification> notifications)
        {
            var humanUnitIds = tribe.HumanUnits
                .Select(x => x.Id)
                .ToList();

            var tasksToConsume = tribe.HumanUnitTasks
                .Where(x => humanUnitIds.Contains(x.HumanUnitId))
                .Where(x => x.To <= currentTimeInLoop)
                .ToList();

            foreach (var task in tasksToConsume) 
            {
                var notification = ConsumeTask(task,
                    tribe.HumanUnits.Single(x => x.Id == task.HumanUnitId), 
                    tribe,
                    mapTiles,
                    currentTimeInLoop,
                    tribe.HumanUnitTaskOrders);

                if (notification != null)
                    notifications.Add(notification);

                tribe.HumanUnitTasks.Remove(task);
            }
        }

        INotification? ConsumeTask(HumanUnitTask humanUnitTask,
            HumanUnit humanUnit,
            Tribe tribe,
            ICollection<MapTile> mapTiles,
            DateTime currentTimeInLoop,
            ICollection<HumanUnitTaskOrder> humanUnitTaskOrders)
        {
            switch (humanUnitTask.Type)
            {
                case EHumanUnitTaskType.GatheringFood:
                    const int MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK = 5;

                    var mapTile = mapTiles.Single(x => x.Localization.Equals(humanUnitTask.Localization));
                    var mapTileFoodPoints = mapTile.Food.ActualPoints;

                    var foodGatheringCoefficient = (double)humanUnit.FoodLevelPercentage / 100 * 2;
                    var foodPoints = foodGatheringCoefficient >= 1 
                        ? MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK
                        : foodGatheringCoefficient * MAX_FOOD_PONTS_GATHERED_BY_ONE_TASK;

                    var gatheredFoodPoints = mapTileFoodPoints > foodPoints 
                        ? foodPoints 
                        : mapTileFoodPoints;

                    tribe.Resources.FreshFood += (int)gatheredFoodPoints;

                    var finishedHumanTaskOrder = humanUnitTaskOrders.Where(x => x.IsInProgress).OrderBy(x => x.Added).FirstOrDefault();
                    
                    if (finishedHumanTaskOrder != null)
                        humanUnitTaskOrders.Remove(finishedHumanTaskOrder);
                    else
                    {
                        //todo add logs - finishedHumanTaskOrder should always exists, if its null its due to problem - happens 2 times.. 
                    }

                    return new NotificationDto(humanUnit.Id, humanUnit.Name, currentTimeInLoop, ENotificationType.FoodGatheringEnded, gatheredFoodPoints.ToString());

                case EHumanUnitTaskType.ConsumeFood:
                    //note: consumption of the food applies when task is created
                    return new NotificationDto(humanUnit.Id, humanUnit.Name, currentTimeInLoop, ENotificationType.FoodConsumptionEnded, null);

                default: throw new NotImplementedException();
            }
        }
    }
}
