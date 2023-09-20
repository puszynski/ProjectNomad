using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
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
        readonly ITribeRelocationService _tribeRelocationService;
        public GameLOOPER(
            IDayExecutor dayExecutor,
            IHourExecutor hourExecutor,
            ISecundExecutor secundExecutor,
            IMinuteExecutor minuteExecutor,
            ITribeRepository tribeRepository,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository,
            IGameOverApplicator gameOverApplicator,
            ITribeRelocationService tribeRelocationService)
        {
            _dayExecutor = dayExecutor;
            _hourExecutor = hourExecutor;
            _secundExecutor = secundExecutor;
            _minuteExecutor = minuteExecutor;
            _tribeRepository = tribeRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapTileRepository = mapTileRepository;
            _gameOverApplicator = gameOverApplicator;
            _tribeRelocationService = tribeRelocationService;
        }

        public async Task<ITriggerGameLooperResponse>  LoopTribe(Guid accountId)
        {

            var tribe = await _tribeRepository.GetAllDataMaterialized(accountId);

            if (!tribe.HumanUnits.Any())
                return GetGameOverResponse(tribe);

            var lastUpdated = tribe.Updated;

            if (lastUpdated > _dateTimeProvider.UtcNow().AddSeconds(-1))
                return GetEmptyResponse(tribe);

            var mapTiles = await GetMapTileToInteract(tribe);
            var notifications = new List<INotification>();

            while (lastUpdated <= _dateTimeProvider.UtcNow().AddSeconds(-1))
            {
                try
                {
                    var currentTimeInLoop = lastUpdated.AddSeconds(1);

                    await _secundExecutor.Execute(tribe,
                        mapTiles,
                        notifications,
                        currentTimeInLoop);

                    //todo - make classes for 2secundExecutor, 5sec, 10sec <- event 2s is 2x less calculation!! it`s worth it!

                    if (lastUpdated.Second == 0)
                        _minuteExecutor.Execute(tribe, notifications);

                    if (IGameOverApplicator.IsGameOver(tribe.HumanUnits))
                        break;

                    if (lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                        _hourExecutor.Execute(tribe, mapTiles, notifications);

                    if (lastUpdated.Hour == 12 && lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                        _dayExecutor.Execute();

                    lastUpdated = currentTimeInLoop;
                }
                catch (Exception ex)
                {
                    //todo log
                    
                    var error = ex;
                }
                
            }

            if (IGameOverApplicator.IsGameOver(tribe.HumanUnits))
            {
                _gameOverApplicator.Execute(tribe.Id);
                tribe.Updated = _dateTimeProvider.UtcNow();
                await _tribeRepository.SaveChangesAsync();

                return GetGameOverResponse(tribe);
            }
            else
            {
                tribe.Updated =  lastUpdated;
                await _tribeRepository.SaveChangesAsync();

                var relocationStatus = _tribeRelocationService.GetTribeRelocationStatus(tribe);

                return new TriggerGameLooperResponse(
                    new TribeDto(tribe.Id, tribe.Name, tribe.Localization.X, tribe.Localization.Y, tribe.Resources.Wood, tribe.Resources.FreshFood, relocationStatus), 
                    tribe.HumanUnits.Select(x => new HumanUnitDto(x.Id, x.Name, x.Localization.X, x.Localization.Y, x.FoodLevelPercentage)), 
                    notifications, 
                    new List<WorldEventDto>());
            }
        }

        async Task<ICollection<MapTile>> GetMapTileToInteract(Tribe tribe)
        {
            var mapTileLocalizationsForTasks = tribe.HumanUnitTasks
                .Where(x => x.Localization != null)
                .Select(y => y.Localization)
                .ToList();

            var mapTileLocalizationsForTaskOrders = tribe.HumanUnitTaskOrders
                .Where(x => x.Localization != null)
                .Select(y => y.Localization)
                .ToList();

            var allMapTileLocalization = mapTileLocalizationsForTasks.Union(mapTileLocalizationsForTaskOrders).ToList();

            return await _mapTileRepository.GetByLocalizations(allMapTileLocalization);
        }

        TriggerGameLooperResponse GetGameOverResponse(Tribe tribe)
            => new(
                new TribeDto(tribe.Id, tribe.Name, tribe.Localization.X, tribe.Localization.Y, tribe.Resources.Wood, tribe.Resources.FreshFood, ETribeRelocationStatus.None),
                new List<HumanUnitDto>(),
                new List<NotificationDto>() 
                { 
                    new NotificationDto(0, 
                        "none", 
                        _dateTimeProvider.UtcNow(), 
                        ProjectNomad.Shared.Enums.ENotificationType.GameOver, 
                        null) 
                },
                new List<WorldEventDto>());

        TriggerGameLooperResponse GetEmptyResponse(Tribe tribe)
            => new(
                new TribeDto(tribe.Id, tribe.Name, tribe.Localization.X, tribe.Localization.Y, tribe.Resources.Wood, tribe.Resources.FreshFood, ETribeRelocationStatus.None),
                tribe.HumanUnits.Select(x => new HumanUnitDto(x.Id, x.Name, x.Localization.X, x.Localization.Y, x.FoodLevelPercentage)),
                new List<NotificationDto>(), 
                new List<WorldEventDto>());
    }
}
