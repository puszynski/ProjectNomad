using GameModule.DtoModels;
using GameModule.Entities;
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
        readonly IGameOverApplicator _gameOverApplicator;
        public GameLOOPER(
            IDayExecutor dayExecutor,
            IHourExecutor hourExecutor,
            ISecundExecutor secundExecutor,
            IMinuteExecutor minuteExecutor,
            ITribeRepository tribeRepository,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository,
            IGameOverApplicator gameOverApplicator)
        {
            _dayExecutor = dayExecutor;
            _hourExecutor = hourExecutor;
            _secundExecutor = secundExecutor;
            _minuteExecutor = minuteExecutor;
            _tribeRepository = tribeRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapTileRepository = mapTileRepository;
            _gameOverApplicator = gameOverApplicator;
        }

        public async Task<ITriggerGameLooperResponse>  LoopTribe(Guid accountId)
        {

            var tribe = await _tribeRepository.GetAllDataMaterialized(accountId);

            if (!tribe.HumanUnits.Any())
                return GetGameOverResponse();
                //throw new NotImplementedException(); //todo?

            var lastUpdated = tribe.Updated;

            if (lastUpdated > _dateTimeProvider.UtcNow().AddSeconds(-1))
                return GetEmptyResponse();

            var mapTiles = await GetMapTileToInteract(tribe);
            var notifications = new List<INotification>();

            while (lastUpdated <= _dateTimeProvider.UtcNow().AddSeconds(-1))
            {
                var currentTimeInLoop = lastUpdated.AddSeconds(1);

                await _secundExecutor.Execute(tribe,
                    mapTiles,
                    notifications,
                    currentTimeInLoop);

                if (lastUpdated.Second == 0)
                    _minuteExecutor.Execute(tribe, notifications);

                if (IGameOverApplicator.IsGameOver(tribe.HumanUnits))
                    break;

                if (lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                    _hourExecutor.Execute();

                if (lastUpdated.Hour == 12 && lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                    _dayExecutor.Execute();

                lastUpdated = currentTimeInLoop;
            }

            if (IGameOverApplicator.IsGameOver(tribe.HumanUnits))
            {
                _gameOverApplicator.Execute(tribe.Id);
                tribe.Updated = _dateTimeProvider.UtcNow();
                await _tribeRepository.SaveChangesAsync();
                return GetGameOverResponse();
            }
            else
            {
                tribe.Updated =  lastUpdated;
                await _tribeRepository.SaveChangesAsync();
                return new TriggerGameLooperResponse(notifications, new List<WorldEventDto>());
            }
        }

        async Task<List<MapTile>> GetMapTileToInteract(Tribe tribe)
        {
            var mapTileIdsForTasks = tribe.HumanUnitTasks
                .Where(x => x.MapTileId.HasValue)
                .Select(y => y.MapTileId.Value)
            .ToList();

            var mapTileIdsForTaskOrders = tribe.HumanUnitTaskOrders
                .Where(x => x.MapTileId.HasValue)
                .Select(y => y.MapTileId.Value)
                .ToList();

            var allMapTileIds = mapTileIdsForTasks.Union(mapTileIdsForTaskOrders).ToList();

            return await _mapTileRepository.GetByIds(allMapTileIds);
        }

        TriggerGameLooperResponse GetGameOverResponse()
            => new(
                new List<NotificationDto>() 
                { 
                    new NotificationDto(0, 
                        "none", 
                        _dateTimeProvider.UtcNow(), 
                        ProjectNomad.Shared.Enums.ENotificationType.GameOver, 
                        null) 
                },
                new List<WorldEventDto>());

        TriggerGameLooperResponse GetEmptyResponse()
            => new(new List<NotificationDto>(), new List<WorldEventDto>());
    }
}
