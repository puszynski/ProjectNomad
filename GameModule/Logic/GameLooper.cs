using GameModule.DtoModels;
using GameModule.Logic.GameLooperLogic;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

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

        public async Task<ITriggerGameLooperResponse>  LoopTribe(Guid accountId)
        {
            var tribe = await _tribeRepository.GetByAccountId(accountId);

            if (tribe == null)
                throw new ArgumentException($"GameLOOPER! There is no tribe assigned for given accountId {accountId}");

            var humanUnits = await _humanUnitRepository.GetHumanUnitsByTribeId(tribe.Id);
            
            if (!humanUnits.Any())
                return new TriggerGameLooperResponse(
                    new List<NotificationDto>() { new NotificationDto(0, _dateTimeProvider.UtcNow(), ProjectNomad.Shared.Enums.ENotificationType.GameOver, null) }, 
                    new List<WorldEventDto>());
            
            var tasksToConsume = await _humanUnitTaskRepository.GetHumanUnitTasksByTribeId(tribe.Id);


            var mapTileIds = tasksToConsume
                .Where(x => x.MapTileId.HasValue)
                .Select(y => y.MapTileId.Value)
                .ToList();

            var mapTilesFromTasks = await _mapTileRepository.GetByIds(mapTileIds);

            var lastUpdated = tribe.Updated;
            var loopCounter = 0;
            var shouldBreakGameLoop = false;

            var notificationToSendToClient = new List<INotification>();

            while (lastUpdated <= _dateTimeProvider.UtcNow().AddSeconds(-1))
            {
                var currentTimeInLoop = lastUpdated.AddSeconds(1);

                var notificationsFromCurrentLoop = await _secundExecutor.Execute(humanUnits,
                    tribe,
                    tasksToConsume,
                    mapTilesFromTasks,
                    currentTimeInLoop);

                notificationToSendToClient.AddRange(notificationsFromCurrentLoop);

                if (lastUpdated.Second == 0)
                    shouldBreakGameLoop = _minuteExecutor.Execute(humanUnits, 
                        tribe, 
                        tasksToConsume);

                if (shouldBreakGameLoop)
                    break;

                if (lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                    _hourExecutor.Execute();

                if (lastUpdated.Hour == 12 && lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                    _dayExecutor.Execute();

                lastUpdated = currentTimeInLoop;
                loopCounter++;
            }

            if (loopCounter != 0)
            {
                tribe.Updated = lastUpdated;
                await _tribeRepository.SaveChangesAsync();
            }

            return new TriggerGameLooperResponse(notificationToSendToClient, new List<WorldEventDto>()); //todo implement WorldEvents
        }
    }
}
