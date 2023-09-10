using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic;
using GameModule.Repositories;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace GameModule
{
    internal class GameModule : IGameModule
    {
        const int TILE_MAX_FOOD_POINTS_LIMIT = 100;
        const int TILE_MAX_WOOD_POINTS_LIMIT = 100;

        readonly NewTribeLocalizationInitializer _newTribeLocalizationInitializer;
        readonly IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IMapTileRepository _mapTileRepository;
        readonly GameModuleDbContext _dbContext;
        readonly GameLOOPER _gameLooper;
        readonly MapService _mapService;

        public GameModule(GameModuleDbContext dbContext,
            NewTribeLocalizationInitializer newTribeLocalizationInitializer,
            IHumanUnitTaskOrderRepository humanUnitTaskOrderRepository,
            GameLOOPER gameLooper,
            MapService mapService,
            IDateTimeProvider dateTimeProvider,
            IMapTileRepository mapTileRepository)
        {
            _dbContext = dbContext;
            _newTribeLocalizationInitializer = newTribeLocalizationInitializer;
            _humanUnitTaskOrderRepository = humanUnitTaskOrderRepository;
            _gameLooper = gameLooper;
            _mapService = mapService;
            _dateTimeProvider = dateTimeProvider;
            _mapTileRepository = mapTileRepository;
        }

        public async Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId)
        {
            var tribe = await _dbContext
                .Tribes
                .Where(x => x.AccountId == accountId)
                .Select(x => new { x.Name, x.Id, x.Localization.X, x.Localization.Y, x.Resources.FreshFood, x.Resources.Wood })
                .SingleOrDefaultAsync()
                    ?? throw new ArgumentException("No tribe founded in database with given accountId :/", nameof(accountId));

            var tribeDto = new TribeDto(tribe.Id, tribe.Name, tribe.X, tribe.Y, tribe.Wood, tribe.FreshFood);

            var humanUnitsTask = _dbContext
                .HumanUnits
                .Where(x => x.TribeId == tribe.Id)
                .Select(x => new HumanUnitDto(x.Id, x.Name, x.Localization.X, x.Localization.Y, x.FoodLevelPercentage))
                .ToListAsync();

            return new TribeGameObjectDto(tribeDto, await humanUnitsTask);
        }

        public async Task<ITriggerGameLooperResponse> TriggerPlayerGameObjectRecalculation(Guid accountId) 
            => await _gameLooper.LoopTribe(accountId);

        public async Task InitPlayerGameObjects(Guid accountId)
        {
            var localization =  await _newTribeLocalizationInitializer.Initialize();

            var tribe = new Tribe()
            {
                AccountId = accountId,
                Name = "Tribe with no name",
                Localization = localization,
                Updated = _dateTimeProvider.UtcNow()
            };

            var humanUnits = new List<HumanUnit>();

            for (int i = 0; i < 4; i++)
                humanUnits.Add(HumanUnitGenerator.Generate(tribe));
                        
            var taskTribe = _dbContext.Tribes.AddAsync(tribe);
            await taskTribe;

            var taskHumanUnits = _dbContext.HumanUnits.AddRangeAsync(humanUnits);
            await taskHumanUnits;
            await _dbContext.SaveChangesAsync();
        }

        public async Task InitPlayerGameObjectsForExistingTribe(Guid accountId)
        {
            var tribe = await GetTribeOrArgumentException(accountId);

            if (await _dbContext.HumanUnits.AnyAsync(x => x.TribeId == tribe.Id))
                throw new ArgumentException($"To init new human units, there should be no in database (tribeId:{tribe.Id}) :/");

            var humanUnits = new List<HumanUnit>();

            for (int i = 0; i < 4; i++)
                humanUnits.Add(HumanUnitGenerator.Generate(tribe));

            await _dbContext.HumanUnits.AddRangeAsync(humanUnits);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<IMapTile>> GetMapTiles(int tribeId)
        {
            var tribe = await GetTribeOrArgumentException(tribeId);

            var x = tribe.Localization.X;
            var y = tribe.Localization.Y;

            var tiles = await _dbContext.MapTiles
                .Where(t => t.Localization.X >= x - 3 && t.Localization.X <= x + 3)
                .Where(t => t.Localization.Y >= y - 3 && t.Localization.Y <= y + 3)
                .OrderBy(t => t.Localization.Y)
                .ThenBy(t => t.Localization.X)
                .Select(t => new MapTileDto(t.Localization.X, t.Localization.Y, t.Type, t.Food.ActualPoints, t.Wood.ActualPoints))
                .ToListAsync();

            if (tiles.Count() != 7 * 7)
                throw new NotImplementedException(); //todo assign ocean to missing ones..

             return tiles;
        }

        public async Task GenerateMapTiles(int x_start, int y_start)
        {
            //todo check if not exists, or override?

            var tiles = await _mapService.GenerateMapTiles(x_start, 
                y_start,
                TILE_MAX_FOOD_POINTS_LIMIT, 
                TILE_MAX_WOOD_POINTS_LIMIT);

            await _dbContext.MapTiles.AddRangeAsync(tiles);
            await _dbContext.SaveChangesAsync();
        }

        async Task<Tribe> GetTribeOrArgumentException(int tribeId)
        {
            var tribe = await _dbContext
                .Tribes
                .SingleOrDefaultAsync(x => x.Id == tribeId);

            if (tribe == null)
                throw new ArgumentException(nameof(tribe), $"No tribe for given id ({tribeId}) existing :/");

            return tribe;
        }

        async Task<Tribe> GetTribeOrArgumentException(Guid accountId)
        {
            var tribe = await _dbContext
                .Tribes
                .SingleOrDefaultAsync(x => x.AccountId == accountId);

            if (tribe == null)
                throw new ArgumentException(nameof(tribe), $"No tribe for given account id ({accountId}) existing :/");

            return tribe;
        }

        public async Task AddTask(IAddHumanUnitTaskDto task)
        {
            var distance = MapTileDistanceCalculator.Execute(task.LocalizationStart_X, 
                task.LocalizationStart_Y, 
                task.LocalizationEnd_X, 
                task.LocalizationEnd_Y);

            var humanUnit = await _dbContext.HumanUnits.SingleAsync(x => x.Id == task.HumanUnitId);
            var mapTile = await _dbContext.MapTiles.SingleAsync(x => x.Localization.X == task.LocalizationStart_X && x.Localization.Y == task.LocalizationStart_Y);

            var timeToEndTask = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, humanUnit.FoodLevelPercentage) + TimeSpan.FromMinutes(GameSETTINGS.Food.MinutesToGatherFood);

            var entity = new HumanUnitTask 
            {
                From = _dateTimeProvider.UtcNow(),
                HumanUnitId = task.HumanUnitId,
                TribeId = task.TribeId,
                Type = task.Type,
                To = _dateTimeProvider.UtcNow().Add(timeToEndTask),
                MapTileId = mapTile.Id
            };

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<IHumanUnitTaskDto>> GetActualTribeTasks(int tribeId)
        {
            var tribeTasks = await _dbContext
                .HumanUnitTasks
                .Where(x => x.TribeId == tribeId)
                .Include(b => b.HumanUnit)
                .Select(x => new HumanUnitTaskDto(x.TribeId, x.HumanUnitId, x.HumanUnit.Name, x.Type, x.From, x.To)) //todo
                .ToListAsync();

            tribeTasks ??= new List<HumanUnitTaskDto>();
            return tribeTasks;
        }


        //TODO START MAKING SMALLER CLASSES, GAME-MODULE IS TOO BIG => IHumanUnitTaskOrder_GameModule ?
        public async Task<IEnumerable<IHumanUnitTaskOrder>> GetHumanUnitTaskOrders(int tribeId)
            => await _dbContext
            .HumanUnitTaskOrders
            .Where(x => x.TribeId == tribeId)
            .Select(x => new HumanUnitTaskOrderDto(x.Id, x.TribeId, x.Added, x.Type, x.IsInProgress, x.MapTileId))
            .ToListAsync();

        public async Task AddHumanUnitTaskOrder(int tribeId, EHumanUnitTaskType type, int mapTileX, int mapTileY)
        {
            int mapTileId = await _mapTileRepository.GetIdByLocalization(mapTileX, mapTileY);
            await _humanUnitTaskOrderRepository.Add(tribeId, type, mapTileId, _dateTimeProvider.UtcNow());
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHumanUnitTaskOrder(int taskOrderId)
        {
            await _mapTileRepository.Remove(taskOrderId);
            await _dbContext.SaveChangesAsync();
        }
    }
}
