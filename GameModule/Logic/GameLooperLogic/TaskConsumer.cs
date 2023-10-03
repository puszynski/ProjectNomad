using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.LooperServices;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using System.Collections.Generic;

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
        readonly IFirecampService _firecampService;
        public TaskConsumer(IFirecampService firecampService)
        {
            _firecampService = firecampService;
        }

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
                    var mapTile = mapTiles.Single(x => x.Localization.Equals(humanUnitTask.Localization));
                    var mapTileFoodPoints = mapTile.Food.ActualPoints;

                    var foodGatheringCoefficient = (double)humanUnit.FoodLevelPercentage / 100 * 2;
                    var foodPoints = foodGatheringCoefficient >= 1 
                        ? GameSETTINGS.Food.MapTileFoodGathered
                        : foodGatheringCoefficient * GameSETTINGS.Food.MapTileFoodGathered;

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

                case EHumanUnitTaskType.GatheringWood:
                    //todo move to other class, name conflicts..
                    mapTile = mapTiles.Single(x => x.Localization.Equals(humanUnitTask.Localization));
                    var mapTileWoodPoints = mapTile.Wood.ActualPoints;

                    var woodGatheringCoefficient = (double)humanUnit.FoodLevelPercentage / 100 * 2;
                    var woodPoints = woodGatheringCoefficient >= 1
                        ? GameSETTINGS.Wood.WoodAmountGatheredFromMap
                        : woodGatheringCoefficient * GameSETTINGS.Wood.WoodAmountGatheredFromMap;

                    var gatheredWoodPoints = mapTileWoodPoints > woodPoints
                        ? woodPoints
                        : mapTileWoodPoints;

                    tribe.Resources.Wood += (int)gatheredWoodPoints;

                    finishedHumanTaskOrder = humanUnitTaskOrders.Where(x => x.Type == EHumanUnitTaskType.GatheringWood && x.IsInProgress).OrderBy(x => x.Added).FirstOrDefault();

                    if (finishedHumanTaskOrder != null)
                        humanUnitTaskOrders.Remove(finishedHumanTaskOrder);
                    else
                    {
                        //todo add logs - finishedHumanTaskOrder should always exists, if its null its due to problem - happens 2 times.. 
                    }

                    return new NotificationDto(humanUnit.Id, humanUnit.Name, currentTimeInLoop, ENotificationType.WoodGatheringEnded, gatheredWoodPoints.ToString());

                case EHumanUnitTaskType.LightAFire:
                    return _firecampService.LightFire_TaskEnd(humanUnit,
                        tribe,
                        currentTimeInLoop,
                        humanUnitTaskOrders);                    

                case EHumanUnitTaskType.KeepLowFire:
                    return _firecampService.KeepFire_TaskEnd(false, 
                        humanUnit,
                        tribe,
                        currentTimeInLoop,
                        humanUnitTaskOrders);
                    //service..

                case EHumanUnitTaskType.KeepFireBig:
                    return _firecampService.KeepFire_TaskEnd(true,
                        humanUnit,
                        tribe,
                        currentTimeInLoop,
                        humanUnitTaskOrders);

                case EHumanUnitTaskType.ConsumeFood:
                    //note: consumption of the food applies when task is created
                    return new NotificationDto(humanUnit.Id, humanUnit.Name, currentTimeInLoop, ENotificationType.FoodConsumptionEnded, null);

                default: throw new NotImplementedException();
            }
        }
    }
}
