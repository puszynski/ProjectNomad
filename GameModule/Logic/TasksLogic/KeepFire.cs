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

        INotification ITask.Start(HumanTaskOrder taskOrder, 
            Tribe tribeMaterializedData, 
            DateTime currentTimeInLoop, 
            IEnumerable<MapTile> mapTiles)
        {
            var fire = tribeMaterializedData
                .TribeStructures
                .SingleOrDefault(x => x.Type == ETribeStructureType.Firecamp);

            if (fire == null
                || fire.PowerAndDurability > FIRE_POWER_TO_ADD_WOOD)
                return default;

            var taskOrderToAssign = tribeMaterializedData.HumanTaskOrders
                    .Where(x => x.Type == ETaskType.KeepFire)
                    .Where(x => !x.HumanTaskId.HasValue)
                    .SingleOrDefault();

            if (taskOrderToAssign == null
                || !RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond)
                || tribeMaterializedData.Resources.Wood < GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning)
                return default;


            var humanUnitToAssign = BasicDataSelector
                .SelectFirstHumanWithCondition(tribeMaterializedData);

            if (humanUnitToAssign == null)
                return default;

            var taskToAdd = new HumanTask
            {
                From = currentTimeInLoop,
                HumanId = humanUnitToAssign.Id,
                Localization = taskOrder.Localization,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Fire.BurningCampFireEch10Seconds),
                TribeId = tribeMaterializedData.Id,
                Type = ETaskType.KeepFire,
                IsCompleted = false,
            };
            tribeMaterializedData.HumanTasks.Add(taskToAdd);
            taskOrder.HumanTask = taskToAdd;
            tribeMaterializedData.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;
            fire.PowerAndDurability = FULL_FIRE_POWER;   

            return new NotificationDto(humanUnitToAssign.Id,
                    humanUnitToAssign.Name,
                    currentTimeInLoop,
                    ENotificationType.KeepFireProceeded,
                    CustomValue: null);
        }
        
        INotification ITask.End(HumanTask taskToEnd, 
            Tribe tribeMaterializedData,
            DateTime currentTimeInLoop,
            IEnumerable<MapTile> mapTiles)
        {
            taskToEnd.IsCompleted = true;//przemyśl flow - LightFire i KepFire w nowym flow
            return default;
        }

    }
}
