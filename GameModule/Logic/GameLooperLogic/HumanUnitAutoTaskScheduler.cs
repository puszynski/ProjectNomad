using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumanUnitAutoTaskScheduler
    {
        Task Execute(List<HumanUnit> humanUnits,
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

        public async Task Execute(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            DateTime currentTimeInLoop)
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
                    await CreateTask(humanUnit.Id, 
                        tribe.Id, 
                        tasksToConsume, 
                        currentTimeInLoop);

                    tribe.Resources.FreshFood -= GameSETTINGS.TribeFoodNeededToFill20PercentageOfHumanUnit;
                }
            }
        }

        async Task CreateTask(int humanUnitId, 
            int tribeId, 
            List<HumanUnitTask> tasksToConsume,
            DateTime currentTimeInLoop)
        {
            var entity = new HumanUnitTask
            {
                From = currentTimeInLoop.AddSeconds(-1),//**
                HumanUnitId = humanUnitId,
                TribeId = tribeId,
                Type = ProjectNomad.Shared.Enums.EHumanUnitTaskType.ConsumeFood,
                To = currentTimeInLoop.AddMinutes(GameSETTINGS.MinutesToConsumeFoodToFill20PercentageOfFood).AddSeconds(-1),//**
                MapTileId = null
            };

            tasksToConsume.Add(entity);
            
            await _humanUnitTaskRepository.AddAsync(entity);
            await _humanUnitTaskRepository.SaveChangesAsync();

            //** note - task is set up for the start time of the loop (currentTimeInLoop is time of the end of the loop)
        }
    }
}
