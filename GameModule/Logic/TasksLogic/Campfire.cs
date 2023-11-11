using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.TasksLogic.Helpers;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.TasksLogic
{
    internal class Campfire : ITask
    {
        private const int FULL_FIRE_POWER = 100;
        private const int FIRE_POWER_TO_ADD_WOOD = 30;


        internal static void CampfireBurning(TribeStructure? tribeStructure)
        {
            if (tribeStructure == null) 
                return;

            if (tribeStructure.PowerAndDurability == 0)
                return;

            tribeStructure.PowerAndDurability -= GameSETTINGS.Fire.BurningCampFireEch10Seconds;
        }

        INotification? ITask.Start(HumanTaskOrder taskOrder, 
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
                    ENotificationType.NoWoodForCampfire,
                    CustomValue: null);
            }

            var firecamp = tribe
                .TribeStructures?
                .SingleOrDefault(x => x.Type == ETribeStructureType.Firecamp);

            if (firecamp != null)
                if (firecamp.PowerAndDurability > FIRE_POWER_TO_ADD_WOOD)
                    return default;

            var taskToAdd = new HumanTask
            {
                From = looperNow,
                HumanId = humanWithConditionToStartNewTask.Id,
                Localization = taskOrder.Localization,
                To = looperNow.AddMinutes(GameSETTINGS.Fire.TimeToCompleteFireUp),
                TribeId = tribe.Id,
                Type = ETaskType.CampfireUp,
                IsCompleted = false,
            };

            tribe.HumanTasks?.Add(taskToAdd);
            taskOrder.HumanTask = taskToAdd;
            tribe.Resources.Wood -= GameSETTINGS.Fire.WoodUsedToKeepTheCampfireBurning;

            return new NotificationDto(humanWithConditionToStartNewTask.Id,
                    humanWithConditionToStartNewTask.Name,
                    looperNow,
                    ENotificationType.AttemptToStartFireStarted,
                    CustomValue: null);
        }

        INotification? ITask.End(HumanTask taskToEnd, 
            Tribe tribe, 
            DateTime looperNow,
            IEnumerable<MapTile> mapTiles)
        {
            if (tribe.Humans == null)
                return default;

            var taskOrder = tribe
                .HumanTaskOrders?
                .Where(x => x.Id == taskToEnd.Id)
                .OrderBy(x => x.Added)
                .FirstOrDefault();

            var firecamp = tribe
                .TribeStructures?
                .SingleOrDefault(x => x.Type == ETribeStructureType.Firecamp);

            if (firecamp == null)
            {
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

                if (tribe.TribeStructures?.Any(x => x.Type == ETribeStructureType.Firecamp) == true)
                {
                    var existingFirecamp = tribe.TribeStructures.Single(x => x.Type == ETribeStructureType.Firecamp);
                    existingFirecamp.PowerAndDurability = FULL_FIRE_POWER;
                }
                else
                {
                    var fire = new TribeStructure()
                    {
                        TribeId = tribe.Id,
                        Type = ETribeStructureType.Firecamp,
                        PowerAndDurability = FULL_FIRE_POWER
                    };
                    tribe.TribeStructures?.Add(fire);
                }

                tribe.HumanTasks?.Remove(taskToEnd); //REMOVING FOR CONTANT TASKS

                return new NotificationDto(taskToEnd.HumanId,
                    tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                    looperNow,
                    ENotificationType.FireStarted,
                    CustomValue: null);
            }
            else
            {
                firecamp.PowerAndDurability = FULL_FIRE_POWER;
                tribe.HumanTasks?.Remove(taskToEnd); //REMOVING FOR CONTANT TASKS

                return new NotificationDto(taskToEnd.HumanId,
                        tribe.Humans.Single(x => x.Id == taskToEnd.HumanId).Name,
                        looperNow,
                        ENotificationType.KeepFireProceeded,
                        CustomValue: null);
            }
        }            
    }
}
