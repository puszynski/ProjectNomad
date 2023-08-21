using GameModule.Logic.GameLooperLogic;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic
{
    internal class GameLOOPER
    {
        readonly IDayExecutor _dayExecutor;
        readonly IHourExecutor _hourExecutor;
        readonly ISecundExecutor _secundExecutor;
        readonly IMinuteExecutor _minuteExecutor;
        readonly ITribeRepository _tribeRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IMapTileRepository _mapTileRepository;
        readonly IHumanUnitRepository _humanUnitRepository;
        readonly IHumanUnitTaskRepository _humanUnitTaskRepository;
        public GameLOOPER(
            IDayExecutor dayExecutor,
            IHourExecutor hourExecutor,
            ISecundExecutor secundExecutor,
            IMinuteExecutor minuteExecutor,
            ITribeRepository tribeRepository,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository,
            IHumanUnitRepository humanUnitRepository,
            IHumanUnitTaskRepository humanUnitTaskRepository)
        {
            _dayExecutor = dayExecutor;
            _hourExecutor = hourExecutor;
            _secundExecutor = secundExecutor;
            _minuteExecutor = minuteExecutor;
            _tribeRepository = tribeRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapTileRepository = mapTileRepository;
            _humanUnitRepository = humanUnitRepository;
            _humanUnitTaskRepository = humanUnitTaskRepository;
        }

        public async Task LoopTribe(Guid accountId)
        {
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

            var test = _dateTimeProvider.UtcNow().AddSeconds(-1);
            while (lastUpdated <= _dateTimeProvider.UtcNow().AddSeconds(-1))
            {
                await _secundExecutor.Execute(humanUnits,
                    tribe,
                    tasksToConsume,
                    mapTilesFromTasks);

                if (lastUpdated.Second == 0)
                    _minuteExecutor.Execute(humanUnits, tribe);

                if (lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                    _hourExecutor.Execute();

                if (lastUpdated.Hour == 12 && lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                    _dayExecutor.Execute();

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
