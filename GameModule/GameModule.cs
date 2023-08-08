using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    internal class GameModule : IGameModule
    {
        const int TILE_MAX_FOOD_POINTS_LIMIT = 100;
        const int TILE_MAX_WOOD_POINTS_LIMIT = 100;

        readonly GameModuleDbContext _dbContext;
        readonly NewTribeLocalizationInitializer _newTribeLocalizationInitializer;
        readonly GameLOOPER _gameLooper;
        readonly MapService _mapService;

        public GameModule(GameModuleDbContext dbContext,
            NewTribeLocalizationInitializer newTribeLocalizationInitializer,
            GameLOOPER gameLooper,
            MapService mapService)
        {
            _dbContext = dbContext;
            _newTribeLocalizationInitializer = newTribeLocalizationInitializer;
            _gameLooper = gameLooper;
            _mapService = mapService;
        }


        public async Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId)
        {
            try
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
                    .Select(x => new HumanUnitDto(x.Name, x.Localization.X, x.Localization.Y, x.FoodLevelPercentage))
                    .ToListAsync();

                return new TribeGameObjectDto(tribeDto, await humanUnitsTask);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task TriggerPlayerGameObjectRecalculation(int tribeId) 
            => await _gameLooper.LoopTribe(tribeId);

        public async Task InitPlayerGameObjects(Guid accountId)
        {
            var localization =  await _newTribeLocalizationInitializer.Initialize();

            var tribe = new Tribe()
            {
                AccountId = accountId,
                Name = "Tribe with no name",
                Localization = localization,
                Updated = DateTime.UtcNow
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

        public async Task InitPlayerGameObjectsForExistingTribe(int tribeId)
        {
            var tribe = await GetTribeOrArgumentException(tribeId);

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

        public async Task AddTask(IHumanUnitTaskDto task)
        {
            //todo test
            const int TIME_MINUTES_TO_COLECT_FOOD = 10;

            var distance = MapTileDistanceCalculator.Execute(task.LocalizationStart_X, 
                task.LocalizationStart_Y, 
                task.LocalizationEnd_X, 
                task.LocalizationEnd_Y);

            var timeToEndTask = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, task.HumanUnit_FoodLevelPercentage) + TimeSpan.FromMinutes(TIME_MINUTES_TO_COLECT_FOOD);

            var entity = new HumanUnitTask 
            {
                From = task.From,
                HumanUnitId = task.HumanUnitId,
                TribeId = task.TribeId,
                Type = task.Type,
                To = task.From.Add(timeToEndTask),
                MapTileId = task.MapTileId
            };

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
