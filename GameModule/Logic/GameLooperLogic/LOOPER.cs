using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.MinuteExecutorLogic;
using GameModule.Logic.GameLooperLogic.SharedExecutorLogic;
using GameModule.Repositories;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;
using ProjectNomad.Shared.DTOs.ServerToWasm;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;
using System;

namespace GameModule.Logic.GameLooperLogic
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
        readonly ITenSecondsExecutor _tenSecondsExecutor;
        readonly ITribeRelocationService _tribeRelocationService;
        readonly IWorldZoneParameterRepository _worldZoneParameterRepository;

        public LOOPER(
            IDayExecutor dayExecutor,
            IHourExecutor hourExecutor,
            ISecundExecutor secundExecutor,
            IMinuteExecutor minuteExecutor,
            ITribeRepository tribeRepository,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository,
            IGameOverApplicator gameOverApplicator,
            ITenSecondsExecutor tenSecondsExecutor,
            ITribeRelocationService tribeRelocationService,
            IWorldZoneParameterRepository worldZoneParameterRepository)
        {
            _dayExecutor = dayExecutor;
            _hourExecutor = hourExecutor;
            _secundExecutor = secundExecutor;
            _minuteExecutor = minuteExecutor;
            _tribeRepository = tribeRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapTileRepository = mapTileRepository;
            _gameOverApplicator = gameOverApplicator;
            _tenSecondsExecutor = tenSecondsExecutor;
            _tribeRelocationService = tribeRelocationService;
            _worldZoneParameterRepository = worldZoneParameterRepository;
        }

        public async Task<ITriggerGameLooperResponse> LoopTribe(Guid accountId)
        {
            var tribe = await _tribeRepository.GetAllDataMaterialized(accountId);

            var worldZoneParameters = await _worldZoneParameterRepository.GetByZone(EWorldZoneParameter.Temperate);//todo get from tribe localization
            if (worldZoneParameters == null)
                worldZoneParameters = await _worldZoneParameterRepository.Create();

            if (tribe.Humans == null || !tribe.Humans.Any())
                return GetGameOverResponse(tribe);

            var notifications = new List<INotification>();
            var lastUpdated = tribe.Updated;

            if (lastUpdated > _dateTimeProvider.UtcNow().AddSeconds(-1))
                return GetResponseModel(tribe,
                    notifications,
                    worldZoneParameters);

            var mapTiles = await GetMapTileToInteract(tribe);

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
                        await _tenSecondsExecutor.Execute(tribe, worldZoneParameters, notifications, currentTimeInLoop);

                    if (lastUpdated.Second == 0)
                        _minuteExecutor.Execute(tribe, notifications);

                    if (IGameOverApplicator.IsGameOver(tribe.Humans))
                        break;

                    if (lastUpdated.Minute == 0 && lastUpdated.Second == 0)
                        _hourExecutor.Execute(tribe, 
                            mapTiles, 
                            worldZoneParameters, 
                            notifications);

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
                await _gameOverApplicator.Execute(tribe);
                tribe.Updated = _dateTimeProvider.UtcNow();

                //https://stackoverflow.com/questions/19295232/how-to-ignore-a-dbupdateconcurrencyexception-when-deleting-an-entity
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        await _tribeRepository.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        //The database operation was expected to affect 1 row(s), but actually affected 0 row(s); data may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=527962 for information on understanding and handling optimistic concurrency exceptions.'
                        saveFailed = true;
                        foreach (var entry in ex.Entries)
                        {
                            entry.State = EntityState.Detached;//NIE POMAGA.. 
                        }
                    }
                } while (saveFailed);

                return GetGameOverResponse(tribe);
            }
            else
            {
                tribe.Updated = lastUpdated;
                await _tribeRepository.SaveChangesAsync();

                return GetResponseModel(tribe, 
                    notifications,
                    worldZoneParameters);
            }
        }

        async Task<ICollection<MapTile>> GetMapTileToInteract(Tribe tribe)
        {
            if (tribe.HumanTasks == null || tribe.HumanTaskOrders == null)
                throw new NullReferenceException();

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

        TriggerGameLooperResponse GetResponseModel(Tribe tribe,
            IEnumerable<INotification> notifications,
            WorldParametersDto worldParameters)
        {
            var relocationStatus = _tribeRelocationService.GetTribeRelocationStatus(tribe);

            var tribeDto = new TribeDto(tribe.Id,
                tribe.Name,
                tribe.Localization.X,
                tribe.Localization.Y,
                tribe.Resources.Wood,
                tribe.Resources.FreshFood,
                tribe.Started,
                relocationStatus);

            var humanUnitDtos = tribe.Humans.Select(x => new HumanUnitDto(x.Id,
                x.Name,
                x.Localization.X,
                x.Localization.Y,
                x.FoodLevelPercentage,
                x.ThermalLevelPercentage));

            if (tribe.HumanTasks == null || tribe.HumanTaskOrders == null || tribe.TribeStructures == null)
                throw new NullReferenceException();

            var humanUnitTaskDtos = tribe.HumanTasks
                .Select(x => new HumanUnitTaskDto(x.Id,
                x.TribeId,
                x.HumanId,
                x.Human.Name,
                x.Type,
                x.From,
                x.To,
                x.IsCompleted));

            var humanUnitTaskOrderDtos = tribe.HumanTaskOrders.Select(x => new HumanUnitTaskOrderDto(x.Id,
                x.TribeId,
                x.Added,
                x.Type,
                x.HumanTaskId,
                x.Localization.X,
                x.Localization.Y));

            var tribeStructureDtos = tribe.TribeStructures.Select(x => new TribeStructuresDto(x.Id,
                x.Type,
                x.PowerAndDurability));

            return new TriggerGameLooperResponse(
                    tribeDto,
                    humanUnitDtos,
                    humanUnitTaskDtos,
                    humanUnitTaskOrderDtos,
                    tribeStructureDtos,
                    notifications,
                    worldParameters);
        }

        TriggerGameLooperResponse GetGameOverResponse(Tribe tribe)
            => new(
                new TribeDto(
                    tribe.Id, 
                    tribe.Name, 
                    tribe.Localization.X, 
                    tribe.Localization.Y, 
                    tribe.Resources.Wood, 
                    tribe.Resources.FreshFood, 
                    tribe.Started,
                    ETribeRelocationStatus.None),
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
                new WorldParametersDto(50, false, false, false, false));

        //TriggerGameLooperResponse GetEmptyResponse(Tribe tribe)
        //    => new(
        //        new TribeDto(tribe.Id, tribe.Name, tribe.Localization.X, tribe.Localization.Y, tribe.Resources.Wood, tribe.Resources.FreshFood, ETribeRelocationStatus.None),
        //        tribe.Humans.Select(x => new HumanUnitDto(x.Id, 
        //            x.Name, 
        //            x.Localization.X, 
        //            x.Localization.Y,
        //            x.FoodLevelPercentage,
        //            x.ThermalLevelPercentage)),
        //        new List<HumanUnitTaskDto>(),
        //        new List<HumanUnitTaskOrderDto>(),
        //        new List<TribeStructuresDto>(),
        //        new List<NotificationDto>(),
        //        new WorldParametersDto(50, false, false, false, false));
    }
}
