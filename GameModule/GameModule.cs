using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using GameModule.Logic.Services;
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

        readonly NewTribeLocalizationInitializer _newTribeLocalizationInitializer;
        readonly IHumanUnitTaskOrderRepository _humanUnitTaskOrderRepository;
        readonly IMapTileRepository _mapTileRepository;
        readonly IDateTimeProvider _dateTimeProvider;
        readonly GameModuleDbContext _dbContext;
        readonly LOOPER _gameLooper;
        readonly MapService _mapService;

        public GameModule(
            NewTribeLocalizationInitializer newTribeLocalizationInitializer,
            IHumanUnitTaskOrderRepository humanUnitTaskOrderRepository,
            IMapTileRepository mapTileRepository,
            IDateTimeProvider dateTimeProvider,
            GameModuleDbContext dbContext,
            LOOPER gameLooper,
            MapService mapService)
        {
            _newTribeLocalizationInitializer = newTribeLocalizationInitializer;
            _humanUnitTaskOrderRepository = humanUnitTaskOrderRepository;
            _mapTileRepository = mapTileRepository;
            _dateTimeProvider = dateTimeProvider;
            _gameLooper = gameLooper;
            _mapService = mapService;
            _dbContext = dbContext;
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
                Updated = _dateTimeProvider.UtcNow(),
                Started = _dateTimeProvider.UtcNow(),
                Resources = new Resources() { FreshFood = GameSETTINGS.InitializeRebornTribe.FoodPoints, Wood = GameSETTINGS.InitializeRebornTribe.WoodPoints }
            };

            var humanUnits = HumanUnitGenerator.GenerateForNewTribe(tribe, humansToGenerate: 4);
                        
            var taskTribe = _dbContext.Tribes.AddAsync(tribe);
            await taskTribe;

            var taskHumanUnits = _dbContext.Humans.AddRangeAsync(humanUnits);
            await taskHumanUnits;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //SqlException: Cannot insert the value NULL into column 'Resources_Wood', table 'ProjectNomadV2.GameModule.Tribes'; column does not allow nulls. INSERT fails.

                //todo logs
                throw ex;
            }
        }

        public async Task InitPlayerGameObjectsForExistingTribe(Guid accountId)
        {
            var tribe = await GetTribeOrArgumentException(accountId);

            tribe.Started = _dateTimeProvider.UtcNow();
            tribe.Resources.FreshFood = GameSETTINGS.InitializeRebornTribe.FoodPoints;
            tribe.Resources.Wood = GameSETTINGS.InitializeRebornTribe.WoodPoints;

            if (await _dbContext.Humans.AnyAsync(x => x.TribeId == tribe.Id))
                throw new ArgumentException($"To init new human units, there should be no in database (tribeId:{tribe.Id}) :/");

            var humans = HumanUnitGenerator.GenerateForNewTribe(tribe, humansToGenerate: 4);

            await _dbContext.Humans.AddRangeAsync(humans);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<IMapTile>> GetMapTiles(int tribeId)
        {
            const int MAP_SIZE_IN_TILES = 7;

            var tribe = await GetTribeOrArgumentException(tribeId);

            var x = tribe.Localization.X;
            var y = tribe.Localization.Y;

            var tilesCountFromCenterToEdge = (MAP_SIZE_IN_TILES - 1) / 2;

            var tiles = await _dbContext.MapTiles
                .Where(t => t.Localization.X >= x - tilesCountFromCenterToEdge && t.Localization.X <= x + tilesCountFromCenterToEdge)
                .Where(t => t.Localization.Y >= y - tilesCountFromCenterToEdge && t.Localization.Y <= y + tilesCountFromCenterToEdge)
                .OrderBy(t => t.Localization.Y)
                .ThenBy(t => t.Localization.X)
                .Select(t => new MapTileDto(t.Localization.X, t.Localization.Y, t.Type, t.Food.ActualPoints, t.Wood.ActualPoints))
                .ToListAsync();

            var allTiles = FillMapWithOceanTiles(tribe.Localization.X, tribe.Localization.Y, tiles, MAP_SIZE_IN_TILES);

            if (allTiles.Count() != MAP_SIZE_IN_TILES * MAP_SIZE_IN_TILES)
                throw new NotImplementedException();

             return allTiles;
        }

        public async Task<IEnumerable<IMapTile>> GetMapTileForMiniMap(int tribeId, int miniMapSizeInTiles = 31)
        {
            if (miniMapSizeInTiles % 2 == 0)
                throw new NotSupportedException($"{nameof(miniMapSizeInTiles)} is even, but must be odd number :/");

            var tribe = await GetTribeOrArgumentException(tribeId);

            var x = tribe.Localization.X;
            var y = tribe.Localization.Y;

            var tilesCountFromCenterToEdge = (miniMapSizeInTiles - 1) / 2;

            var tiles = await _dbContext.MapTiles
                .Where(t => t.Localization.X >= x - tilesCountFromCenterToEdge && t.Localization.X <= x + tilesCountFromCenterToEdge)
                .Where(t => t.Localization.Y >= y - tilesCountFromCenterToEdge && t.Localization.Y <= y + tilesCountFromCenterToEdge)
                .OrderBy(t => t.Localization.Y)
                .ThenBy(t => t.Localization.X)
                .Select(t => new MapTileDto(t.Localization.X, t.Localization.Y, t.Type, t.Food.ActualPoints, t.Wood.ActualPoints))
                .ToListAsync();

            var allMapTiles = FillMapWithOceanTiles(tribe.Localization.X, tribe.Localization.Y, tiles, miniMapSizeInTiles);

            if (allMapTiles.Count() != miniMapSizeInTiles * miniMapSizeInTiles)
                throw new NotImplementedException();
            
            return allMapTiles;
        }

        List<IMapTile> FillMapWithOceanTiles(int tribeLocalizationX, 
            int tribeLocalizationY, 
            List<MapTileDto> tiles, 
            int squareMapSizeInTiles)
        {
            var tilesCountFromCenterToEdge = (squareMapSizeInTiles - 1) / 2;

            var tilesInOrder = new List<IMapTile>();

            for (int X = tribeLocalizationX - tilesCountFromCenterToEdge; X <= tribeLocalizationX + tilesCountFromCenterToEdge; X++)
                for (int Y = tribeLocalizationY - tilesCountFromCenterToEdge; Y <= tribeLocalizationY + tilesCountFromCenterToEdge; Y++)
                    if (!tiles.Any(x => x.X == X && x.Y == Y))
                        tilesInOrder.Add(new MapTileDto(X, Y, EMapType.Ocean, 0, 0));
                    else
                        tilesInOrder.Add(tiles.Single(x => x.X == X && x.Y == Y));

            return tilesInOrder;
        }

        public async Task GenerateMapTiles(int x_start, int y_start)
        {
            //todo check if not exists, or override?
            var tiles = await _mapService.GenerateMapTiles(x_start, 
                y_start);

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

            var humanUnit = await _dbContext.Humans.SingleAsync(x => x.Id == task.HumanUnitId);

            var timeToEndTask = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, humanUnit.FoodLevelPercentage) + TimeSpan.FromMinutes(GameSETTINGS.Food.MinutesToGatherFood);

            var entity = new HumanTask 
            {
                From = _dateTimeProvider.UtcNow(),
                HumanId = task.HumanUnitId,
                TribeId = task.TribeId,
                Type = task.Type,
                To = _dateTimeProvider.UtcNow().Add(timeToEndTask),
                Localization = new Entities.ValueObjects.Localization { X = task.LocalizationStart_X, Y = task.LocalizationStart_Y }
            };

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<IHumanUnitTaskDto>> GetActualTribeTasks(int tribeId)
        {
            var tribeTasks = await _dbContext
                .HumanTasks
                .Where(x => x.TribeId == tribeId)
                .Include(b => b.Human)
                .Select(x => new HumanUnitTaskDto(x.Id, x.TribeId, x.HumanId, x.Human.Name, x.Type, x.From, x.To, x.IsCompleted))
                .ToListAsync();

            tribeTasks ??= new List<HumanUnitTaskDto>();
            return tribeTasks;
        }

        public async Task<IEnumerable<IHumanUnitTaskOrder>> GetHumanUnitTaskOrders(int tribeId)
            => await _dbContext
            .HumanTaskOrders
            .Where(x => x.TribeId == tribeId)
            .Select(x => new HumanUnitTaskOrderDto(x.Id, x.TribeId, x.Added, x.Type, x.HumanTaskId, x.Localization.X, x.Localization.Y))
            .ToListAsync();

        public async Task AddHumanUnitTaskOrder(int tribeId, 
            ETaskType type, 
            int mapTileX, 
            int mapTileY)
        {
            if (type == ETaskType.TribeRelocation)
            {
                var tribe = await GetTribeOrArgumentException(tribeId);
                if (tribe.TribeRelocation != null)
                    throw new ArgumentException($"Can not assign task order for {nameof(ETaskType.TribeRelocation)} when {nameof(tribe.TribeRelocation)} exists :/");
            }

            await _humanUnitTaskOrderRepository.Add(tribeId, type, mapTileX, mapTileY, _dateTimeProvider.UtcNow());
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHumanUnitTaskOrder(int taskOrderId)
        {
            await _mapTileRepository.Remove(taskOrderId);
            await _dbContext.SaveChangesAsync();
        }
    }
}
