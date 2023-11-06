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
        internal static void CampfireBurning(TribeStructure? tribeStructure)
        {
            if (tribeStructure == null) 
                return;

            if (tribeStructure.PowerAndDurability == 0)
                return;

            tribeStructure.PowerAndDurability -= GameSETTINGS.Fire.BurningCampFireEch10Seconds;
        }

        INotification ITask.Start(HumanTaskOrder taskOrder, 
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

            var taskToAdd = new HumanTask
            {
                From = looperNow,
                HumanId = humanWithConditionToStartNewTask.Id,
                Localization = taskOrder.Localization,
                To = looperNow.AddMinutes(GameSETTINGS.Fire.TimeToCompleteAttemptToStartFire),
                TribeId = tribe.Id,
                Type = ETaskType.LightAFire,
                IsCompleted = false,
            };

            tribe.HumanTasks.Add(taskToAdd);
            taskOrder.HumanTask = taskToAdd;
            tribe.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;

            return new NotificationDto(humanWithConditionToStartNewTask.Id,
                    humanWithConditionToStartNewTask.Name,
                    looperNow,
                    ENotificationType.AttemptToStartFireStarted,
                    CustomValue: null);
        }

        INotification ITask.End(HumanTask taskToEnd, 
            Tribe tribe, 
            DateTime looperNow,
            IEnumerable<MapTile> mapTiles)
        {
            var taskOrder = tribe.HumanTaskOrders
                .Where(x => x.Id == taskToEnd.Id)
                .OrderBy(x => x.Added)
                .FirstOrDefault(); 

            if (!RandomCalculator.GetBoolWithGivenProbability(GameSETTINGS.Fire.ChanceToStartFire))
            {
                if (taskOrder != null)
                    taskOrder.HumanTask = null;

                return new NotificationDto(taskToEnd.HumanId,
                    tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
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

            taskToEnd.IsCompleted = true;

            return new NotificationDto(taskToEnd.HumanId,
                tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                looperNow,
                ENotificationType.FireStarted,
                CustomValue: null);
        }            
    }
}
