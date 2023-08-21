using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumanUnitAutoTaskScheduler
    {
        Task Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume);
    }

    internal class HumanUnitAutoTaskScheduler : IHumanUnitAutoTaskScheduler
    {
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        readonly IDateTimeProvider _timeProvider;
        public HumanUnitAutoTaskScheduler(IHumanUnitTaskRepository humanUnitTaskRepository, 
            IDateTimeProvider timeProvider)
        {
            _humanUnitTaskRepository = humanUnitTaskRepository;
            _timeProvider = timeProvider;
        }

        public async Task Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume)
        {
            var humanUnitIdsWithTaskInProgress = tasksToConsume
                .Select(x => x.HumanUnitId)
                .ToList();

            var humanUnitWithNoTasksAndFoodLevelLessThen20 = humanUnits
                .Where(x => !humanUnitIdsWithTaskInProgress.Contains(x.Id))
                .Where(x => x.FoodLevelPercentage <= 80)
                .ToList();

            foreach (var humanUnit in humanUnitWithNoTasksAndFoodLevelLessThen20)
            {
                if (tribe.Resources.FreshFood >= GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit)
                {
                    await CreateTask(humanUnit.Id, tribe.Id, tasksToConsume);
                    tribe.Resources.FreshFood -= GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit;
                }
            }
        }

        async Task CreateTask(int humanUnitId, 
            int tribeId, 
            List<HumanUnitTask> tasksToConsume)
        {
            var entity = new HumanUnitTask
            {
                From = _timeProvider.UtcNow(), // DateTime.UtcNow,
                HumanUnitId = humanUnitId,
                TribeId = tribeId,
                Type = ProjectNomad.Shared.Enums.EHumanUnitTaskType.ConsumeFood,
                To = _timeProvider.UtcNow().AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood),
                MapTileId = null
            };

            tasksToConsume.Add(entity);
            await _humanUnitTaskRepository.AddAsync(entity);
            await _humanUnitTaskRepository.SaveChangesAsync();
        }
    }
}
