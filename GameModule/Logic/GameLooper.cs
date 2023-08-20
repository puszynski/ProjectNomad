using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic
{
    internal class GameLOOPER
    {
        readonly ITribeRepository _tribeRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IMapTileRepository _mapTileRepository;
        readonly IHumanUnitRepository _humanUnitRepository;
        readonly IHumanUnitTaskConsumer _humanUnitTaskConsumer;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        readonly IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        public GameLOOPER(
            ITribeRepository tribeRepository,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository,
            IHumanUnitRepository humanUnitRepository,
            IHumanUnitTaskConsumer humanUnitTaskConsumer,
            IHumanUnitTaskRepository humanUnitTaskRepository,
            IHumanUnitAutoTaskScheduler humanUnitAutoTaskScheduler)
        {
            _tribeRepository = tribeRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapTileRepository = mapTileRepository;
            _humanUnitRepository = humanUnitRepository;
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
            _humanUnitTaskRepository = humanUnitTaskRepository;
            _humanUnitAutoTaskScheduler = humanUnitAutoTaskScheduler;
        }
        async Task ActionsPerSecond(List<HumanUnit> humanUnits,
            Tribe tribe,
            List<HumanUnitTask> tasksToConsume,
            List<MapTile> mapTilesToConsumeTasks)
        {
            var humanUnitAutoTaskSchedulerTask = _humanUnitAutoTaskScheduler.Execute(humanUnits,
                tribe,
                tasksToConsume);

            _humanUnitTaskConsumer.Execute(humanUnits,
                tribe,
                tasksToConsume,
                mapTilesToConsumeTasks);

            await humanUnitAutoTaskSchedulerTask;
        }

        void ActionsPerMinute(List<HumanUnit> humanUnits, 
            Tribe tribe)
        {
            humanUnits.ForEach(x => x.FoodLevelPercentage = x.FoodLevelPercentage - GameSETTINGS.FoodToGetHungryForHumanUnitEachMinute);
            DeathApplicator.StarvationDeath(_humanUnitRepository, humanUnits);
        }

        void ActionsPerHour(List<HumanUnit> humanUnits)
        {
        }

        void ActionsPerDay(List<HumanUnit> humanUnits) 
        {
        }

        public async Task LoopTribe(Guid accountId)
        {
            //todo for repos? - use selects + dto to limit data fetch
            var tribe = await _tribeRepository.GetByAccountId(accountId);

            if (tribe == null) 
                return;

            var humanUnits_task = _humanUnitRepository.GetHumanUnitsByTribeId(tribe.Id);
            var tasksToConsume_task = _humanUnitTaskRepository.GetHumanUnitTasksByTribeId(tribe.Id);

            var humanUnits = await humanUnits_task;
            var tasksToConsume = await tasksToConsume_task;

            if (!humanUnits.Any())
                return;

            var mapTilesFromTasks = await _mapTileRepository.GetByIds(tasksToConsume.Select(y => y.MapTileId).ToList());


            var lastUpdated = tribe.Updated;
            var loopCounter = 0;

            while (lastUpdated <= _dateTimeProvider.UtcNow())
            {
                await ActionsPerSecond(humanUnits, 
                    tribe, 
                    tasksToConsume, 
                    mapTilesFromTasks);

                if (lastUpdated.Second == 0)
                    ActionsPerMinute(humanUnits, tribe);

                if (lastUpdated.Minute == 0)
                    ActionsPerHour(humanUnits);

                if (lastUpdated.Hour == 12)
                    ActionsPerDay(humanUnits);

                lastUpdated = lastUpdated.AddSeconds(1);
                loopCounter++;

            }

            if (loopCounter != 0)
            {
                tribe.Updated = lastUpdated;
                await _tribeRepository.SaveChangesAsync();
            }
        }
    }
}
