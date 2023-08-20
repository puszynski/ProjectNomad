using GameModule.Configurations;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using ProjectNomad.Shared;

namespace GameModule.Logic
{
    internal class GameLOOPER
    {
        readonly GameModuleDbContext _dbContext;
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IHumanUnitTaskConsumer _humanUnitTaskConsumer;
        readonly IHumanUnitAutoTaskScheduler _humanUnitAutoTaskScheduler;
        public GameLOOPER(GameModuleDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            IHumanUnitTaskConsumer humanUnitTaskConsumer,
            IHumanUnitAutoTaskScheduler humanUnitAutoTaskScheduler)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
            _humanUnitAutoTaskScheduler = humanUnitAutoTaskScheduler;
        }
        async Task SecondLooper(List<HumanUnit> humanUnits,
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

        void MinuteLooper(List<HumanUnit> humanUnits, 
            Tribe tribe)
        {
            humanUnits.ForEach(x => x.FoodLevelPercentage = x.FoodLevelPercentage - GameSETTINGS.FoodToGetHungryForHumanUnitEachMinute);
            DeathApplicator.StarvationDeath(_dbContext, humanUnits);
        }

        void HourLooper(List<HumanUnit> humanUnits)
        {
        }

        void DayLooper(List<HumanUnit> humanUnits) 
        {
        }

        public async Task LoopTribe(Guid accountId)
        {
            var t2 = _dbContext.Tribes.ToList();
            //todo try async query materializations.. - there were problems..


            var tribe = _dbContext
                .Tribes
                .SingleOrDefault(x => x.AccountId == accountId);

            var humanUnits = _dbContext
                .HumanUnits
                .Where(x => x.TribeId == tribe.Id)
                .ToList();

            var tasksToConsume = _dbContext
                .HumanUnitTasks
                .Where(x => x.TribeId == tribe.Id)
                .ToList();

            var mapTilesFromTasks = _dbContext
                .MapTiles
                .Where(x => tasksToConsume.Select(y => y.MapTileId).Contains(x.Id))
                .ToList();

            if (tribe == null || !humanUnits.Any())
                return;

            var lastUpdated = tribe.Updated;
            var loopCounter = 0;

            while (lastUpdated <= _dateTimeProvider.UtcNow())
            {
                await SecondLooper(humanUnits, tribe, tasksToConsume, mapTilesFromTasks);

                if (lastUpdated.Second == 0)
                    MinuteLooper(humanUnits, tribe);

                if (lastUpdated.Minute == 0)
                    HourLooper(humanUnits);

                if (lastUpdated.Hour == 0)
                    DayLooper(humanUnits);

                lastUpdated = lastUpdated.AddSeconds(1);
                loopCounter++;

            }

            if (loopCounter != 0)
            {
                tribe.Updated = lastUpdated;
                _dbContext.SaveChanges();
            }
        }
    }
}
