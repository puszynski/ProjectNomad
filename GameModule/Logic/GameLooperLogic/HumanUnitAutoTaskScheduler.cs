using GameModule.DtoModels;
using GameModule.Entities;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumanUnitAutoTaskScheduler
    {
        Task Execute(Tribe tribe,
            List<INotification> notifications,
            DateTime currentTimeInLoop);
    }

    internal class HumanUnitAutoTaskScheduler : IHumanUnitAutoTaskScheduler
    {
        public async Task Execute(Tribe tribe,
            List<INotification> notifications,
            DateTime currentTimeInLoop)
        {
            var humanUnitIdsWithTaskInProgress = tribe.HumanUnitTasks
                .Select(x => x.HumanUnitId)
                .ToList();

            var humanUnitWithNoTasksInProgressAndFoodLevelLessThen80 = tribe.HumanUnits
                .Where(x => !humanUnitIdsWithTaskInProgress.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage <= 80)
                .ToList();

            foreach (var humanUnit in humanUnitWithNoTasksInProgressAndFoodLevelLessThen80)
            {
                if (tribe.Resources.FreshFood >= GameSETTINGS.Food.TribeFoodNeededToFill20PercentageOfHumanUnit)
                {
                    var notification = await CreateTask(humanUnit, 
                        tribe.Id, 
                        tribe.HumanUnitTasks, 
                        currentTimeInLoop);

                    tribe.Resources.FreshFood -= GameSETTINGS.Food.TribeFoodNeededToFill20PercentageOfHumanUnit;
                    humanUnit.FoodLevelPercentage += 20;
                    notifications.Add(notification);
                }
            }
        }

        async Task<INotification> CreateTask(HumanUnit humanUnit, 
            int tribeId, 
            ICollection<HumanUnitTask> tasksToConsume,
            DateTime currentTimeInLoop)
        {
            var entity = new HumanUnitTask
            {
                From = currentTimeInLoop.AddSeconds(-1),//todo remove -1 due to TaskRequire mechanism and change UT
                HumanUnitId = humanUnit.Id,
                TribeId = tribeId,
                Type = EHumanUnitTaskType.ConsumeFood,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.Food.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(-1),//todo remove -1 TaskRequire mechanism and change UT
                Localization = null
            };

            tasksToConsume.Add(entity);
            
            return new NotificationDto(humanUnit.Id, 
                humanUnit.Name, 
                currentTimeInLoop, 
                ENotificationType.FoodConsumptionStarted, 
                entity.To.ToString());
        }
    }
}
