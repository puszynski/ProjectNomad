using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class LightFire : ITask
    {
        private const int FIRE_POWER_AFTER_LIGHT_A_FIRE = 50;
        internal static void CampfireBurning(TribeStructure tribeStructure)
        {
            if (tribeStructure == null) 
                return;

            if (tribeStructure.PowerAndDurability == 0)
                return;

            tribeStructure.PowerAndDurability -= 1;
        }

        INotification ITask.Start(HumanUnitTaskOrder taskOrder, 
            Tribe tribe, 
            DateTime looperNow,
            IEnumerable<MapTile> mapTiles)
        {
            var humanWithConditionToStartNewTask = BasicDataSelector.SelectFirstHumanWithCondition(tribe);

            if (humanWithConditionToStartNewTask == null)
                return default;

            if (!RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.BasicProbabilityToAssignToTaskOrderPerSecond))
                return default;

            if (tribe.Resources.Wood < GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning)
            {
                return new NotificationDto(humanWithConditionToStartNewTask.Id,
                    humanWithConditionToStartNewTask.Name,
                    looperNow,
                    ENotificationType.NoWoodToLightFire,
                    CustomValue: null);
            }

            var taskToAdd = new HumanUnitTask
            {
                From = looperNow,
                HumanUnitId = humanWithConditionToStartNewTask.Id,
                Localization = taskOrder.Localization,
                To = looperNow.AddMinutes(GameSETTINGS.Fire.TimeToCompleteAttemptToStartFire),
                TribeId = tribe.Id,
                Type = EHumanUnitTaskType.LightAFire,
            };

            tribe.HumanUnitTasks.Add(taskToAdd);
            taskOrder.IsInProgress = true;
            tribe.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;

            return new NotificationDto(humanWithConditionToStartNewTask.Id,
                    humanWithConditionToStartNewTask.Name,
                    looperNow,
                    ENotificationType.AttemptToStartFireStarted,
                    CustomValue: null);
        }

        INotification ITask.End(HumanUnitTask taskToEnd, 
            Tribe tribe, 
            DateTime looperNow,
            IEnumerable<MapTile> mapTiles)
        {
            var taskOrder = tribe.HumanUnitTaskOrders
                .Where(x => x.Type == EHumanUnitTaskType.LightAFire && x.IsInProgress)
                .OrderBy(x => x.Added)
                .FirstOrDefault(); 

            if (!RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.Fire.ChanceToStartFire))
            {
                if (taskOrder != null)
                    taskOrder.IsInProgress = false;

                return new NotificationDto(taskToEnd.HumanUnitId,
                    tribe.HumanUnits.Single(x => x.Id == taskToEnd.HumanUnitId).Name,
                    looperNow,
                    ENotificationType.AttemptToStartFireFailed,
                    CustomValue: null);
            };

            if (tribe.TribeStructures.Any(x => x.Type == ETribeStructureType.Firecamp))
            {
                var existingFirecamp = tribe.TribeStructures.Single(x => x.Type == ETribeStructureType.Firecamp);
                existingFirecamp.PowerAndDurability = FIRE_POWER_AFTER_LIGHT_A_FIRE;
            }
            else
            {
                var fire = new TribeStructure()
                {
                    TribeId = tribe.Id,
                    Type = ETribeStructureType.Firecamp,
                    PowerAndDurability = FIRE_POWER_AFTER_LIGHT_A_FIRE
                };
                tribe.TribeStructures.Add(fire);
            }

            return new NotificationDto(taskToEnd.HumanUnitId,
                tribe.HumanUnits.Single(x => x.Id == taskToEnd.HumanUnitId).Name,
                looperNow,
                ENotificationType.FireStarted,
                CustomValue: null);
        }            
    }
}
