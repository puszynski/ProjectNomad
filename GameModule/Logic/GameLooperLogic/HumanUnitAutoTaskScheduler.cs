using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumanUnitAutoTaskScheduler
    {
        Task<IEnumerable<INotification>> Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            DateTime currentTimeInLoop);
    }

    internal class HumanUnitAutoTaskScheduler : IHumanUnitAutoTaskScheduler
    {
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        public HumanUnitAutoTaskScheduler(IHumanUnitTaskRepository humanUnitTaskRepository)
        {
            _humanUnitTaskRepository = humanUnitTaskRepository;
        }

        public async Task<IEnumerable<INotification>> Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            DateTime currentTimeInLoop)
        {
            var humanUnitIdsWithTaskInProgress = tasksToConsume
                .Select(x => x.HumanUnitId)
                .ToList();

            var humanUnitWithNoTasksInProgressAndFoodLevelLessThen80 = humanUnits
                .Where(x => !humanUnitIdsWithTaskInProgress.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage <= 80)
                .ToList();

            var notifications = new List<INotification>();

            foreach (var humanUnit in humanUnitWithNoTasksInProgressAndFoodLevelLessThen80)
            {
                if (tribe.Resources.FreshFood >= GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit)
                {
                    var notification = await CreateTask(humanUnit, 
                        tribe.Id, 
                        tasksToConsume, 
                        currentTimeInLoop);

                    tribe.Resources.FreshFood -= GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit;
                    humanUnit.FoodLevelPercentage += 20;
                    notifications.Add(notification);
                }
            }

            return notifications;
        }

        async Task<INotification> CreateTask(HumanUnit humanUnit, 
            int tribeId, 
            List<HumanUnitTask> tasksToConsume,
            DateTime currentTimeInLoop)
        {
            var entity = new HumanUnitTask
            {
                From = currentTimeInLoop.AddSeconds(-1),//todo remove it due to TaskRequire mechanism and change UT
                HumanUnitId = humanUnit.Id,
                TribeId = tribeId,
                Type = EHumanUnitTaskType.ConsumeFood,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(-1),//todo remove it TaskRequire mechanism and change UT
                MapTileId = null
            };

            tasksToConsume.Add(entity);
            
            await _humanUnitTaskRepository.AddAsync(entity);
            await _humanUnitTaskRepository.SaveChangesAsync();

            return new NotificationDto(humanUnit.Id, 
                humanUnit.Name, 
                currentTimeInLoop, 
                ENotificationType.FoodConsumptionStarted, 
                entity.To.ToString());
        }
    }
}
