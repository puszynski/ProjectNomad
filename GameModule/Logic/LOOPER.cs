using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using GameModule.Logic.TasksLogic;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace GameModule.Logic
{
    internal class LOOPER
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

        GameModuleDbContext gameModuleDbContext;
        public LOOPER(
            IDayExecutor dayExecutor,
            IHourExecutor hourExecutor,
            ISecundExecutor secundExecutor,
            IMinuteExecutor minuteExecutor,
            ITribeRepository tribeRepository,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository,
            IGameOverApplicator gameOverApplicator,
            ITribeRelocationService tribeRelocationService,
            GameModuleDbContext gameModuleDbContext)
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
            this.gameModuleDbContext = gameModuleDbContext;
        }

        public async Task<ITriggerGameLooperResponse>  LoopTribe(Guid accountId)
        {
            var tribe = await _tribeRepository.GetAllDataMaterialized(accountId);
            //REMOVE KEEP FIRE ORDER WHEN FIRE IS OFF???

            if (!tribe.Humans.Any()) 
                return GetGameOverResponse(tribe);

            var lastUpdated = tribe.Updated;

            if (lastUpdated > _dateTimeProvider.UtcNow().AddSeconds(-1))
                return GetEmptyResponse(tribe);//UWAGA!!! JAKIE EMPTY - STĄD ZACIĄGASZ DANE DO WYŚWIETLENIA!!
                // CO ZROBIĆ? MOŻE JAKIŚ WYJĄTEK ALBO KOD BŁĘDU I OBSŁUŻYĆ W KLIENTCIE?
                //MOŻE NULL?? 
                //A MOŻE JEDNAK TRZEBA POBRAĆ DANE I WYSŁAĆ?
                //W A Ż N E

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

                    if (lastUpdated.Second % 10 == 0)
                    {

                        var campfire = tribe
                            .TribeStructures
                            .SingleOrDefault(x => x.Type == ProjectNomad.Shared.Enums.ETribeStructureType.Firecamp);
                        LightFire.CampfireBurning(campfire);
                    }

                    if (lastUpdated.Second == 0)
                        _minuteExecutor.Execute(tribe, notifications);

                    if (IGameOverApplicator.IsGameOver(tribe.Humans))
                        break;

                    if (lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                        _hourExecutor.Execute(tribe, mapTiles, notifications);

                    if (lastUpdated.Hour == 12 && lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                        _dayExecutor.Execute();

                    lastUpdated = currentTimeInLoop;
                }
                catch (Exception ex)
                {
                    var error = ex;
                    throw new Exception(ex.Message + "STACK TRACE: " + ex.StackTrace);
                }
            }

            if (IGameOverApplicator.IsGameOver(tribe.Humans))
            {
                _gameOverApplicator.Execute(tribe);
                tribe.Updated = _dateTimeProvider.UtcNow();
                await _tribeRepository.SaveChangesAsync();
                return GetGameOverResponse(tribe);
            }
            else
            {
                tribe.Updated =  lastUpdated;
                await _tribeRepository.SaveChangesAsync();

                var relocationStatus = _tribeRelocationService.GetTribeRelocationStatus(tribe);

                var tribeDto = new TribeDto(tribe.Id, 
                    tribe.Name, 
                    tribe.Localization.X, 
                    tribe.Localization.Y, 
                    tribe.Resources.Wood, 
                    tribe.Resources.FreshFood, 
                    relocationStatus);

                var humanUnitDtos = tribe.Humans.Select(x => new HumanUnitDto(x.Id, 
                    x.Name, 
                    x.Localization.X, 
                    x.Localization.Y, 
                    x.FoodLevelPercentage));

                var humanUnitTaskDtos = tribe.HumanTasks.Select(x => new HumanUnitTaskDto(x.Id,
                    x.TribeId,
                    x.HumanId,
                    x.Human.Name,
                    x.Type,
                    x.From,
                    x.To));

                var humanUnitTaskOrderDtos = tribe.HumanTaskOrders.Select(x => new HumanUnitTaskOrderDto(x.Id, 
                    x.TribeId, 
                    x.Added, 
                    x.Type, 
                    x.HumanTaskId, 
                    x.Localization.X, 
                    x.Localization.Y));

                var TribeStructureDtos = tribe.TribeStructures.Select(x => new TribeStructuresDto(x.Id,
                    x.Type,
                    x.PowerAndDurability));

                return new TriggerGameLooperResponse(
                    tribeDto,
                    humanUnitDtos,
                    humanUnitTaskDtos,
                    humanUnitTaskOrderDtos,
                    TribeStructureDtos,
                    notifications, 
                    new List<WorldEventDto>());
            }
        }

        async Task<ICollection<MapTile>> GetMapTileToInteract(Tribe tribe)
        {
            var mapTileLocalizationsForTasks = tribe.HumanTasks
                .Where(x => x.Localization != null)
                .Select(y => y.Localization)
                .ToList();

            var mapTileLocalizationsForTaskOrders = tribe.HumanTaskOrders
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
                new List<HumanUnitTaskDto>(),
                new List<HumanUnitTaskOrderDto>(),
                new List<TribeStructuresDto>(),
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
                tribe.Humans.Select(x => new HumanUnitDto(x.Id, x.Name, x.Localization.X, x.Localization.Y, x.FoodLevelPercentage)),
                new List<HumanUnitTaskDto>(),
                new List<HumanUnitTaskOrderDto>(),
                new List<TribeStructuresDto>(),
                new List<NotificationDto>(), 
                new List<WorldEventDto>());
    }
}
