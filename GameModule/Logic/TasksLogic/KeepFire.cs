using GameModule.Entities;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;
using GameModule.DtoModels;
using GameModule.Logic.TasksLogic.Helpers;

namespace GameModule.Logic.TasksLogic
{
    internal class KeepFire : ITask
    {
        private const int FIRE_POWER_TO_ADD_WOOD = 30;
        private const int FULL_FIRE_POWER = 100;

        INotification ITask.Start(HumanUnitTaskOrder taskOrder, 
            Tribe tribeMaterializedData, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            var firecamp = tribeMaterializedData
                .TribeStructures
                .SingleOrDefault(x => x.Type == ETribeStructureType.Firecamp);

            if (firecamp == null
                || firecamp.PowerAndDurability > FIRE_POWER_TO_ADD_WOOD)
                return default;

            var taskOrderToAssign = tribeMaterializedData.HumanUnitTaskOrders
                    .Where(x => x.Type == EHumanUnitTaskType.KeepFire)
                    .Where(x => !x.IsInProgress)
                    .SingleOrDefault();

            if (taskOrderToAssign == null
                || !RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond)
                || tribeMaterializedData.Resources.Wood < GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning)
                return default;


            var humanUnitToAssign = BasicDataSelector
                .SelectFirstHumanWithCondition(tribeMaterializedData);

            if (humanUnitToAssign == null)
                return default;

            var taskToAdd = new HumanUnitTask
            {
                From = currentTimeInLoop,
                HumanUnitId = humanUnitToAssign.Id,
                Localization = taskOrder.Localization,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Fire.MinutesToKeepTheCampfireBurning),
                TribeId = tribeMaterializedData.Id,
                Type = EHumanUnitTaskType.KeepFire,
            };
            tribeMaterializedData.HumanUnitTasks.Add(taskToAdd);
            taskOrder.IsInProgress = true;
            tribeMaterializedData.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;
            firecamp.PowerAndDurability = FULL_FIRE_POWER;   

            return new NotificationDto(humanUnitToAssign.Id,
                    humanUnitToAssign.Name,
                    currentTimeInLoop,
                    ENotificationType.KeepFireProceeded,
                    CustomValue: null);
        }
        
        INotification ITask.End(HumanUnitTask taskToEnd, 
            Tribe tribeMaterializedData,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            return default;
        }

    }
}
