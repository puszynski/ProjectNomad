using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    internal class GameModule : IGameModule
    {
        const int TILE_MAX_FOOD_POINTS_LIMIT = 100;
        const int TILE_MAX_WOOD_POINTS_LIMIT = 100;

        readonly GameModuleDbContext _dbContext;
        readonly NewTribeLocalizationInitializer _newTribeLocalizationInitializer;
        readonly GameLooper _gameLooper;
        readonly MapService _mapService;

        public GameModule(GameModuleDbContext dbContext,
            NewTribeLocalizationInitializer newTribeLocalizationInitializer,
            GameLooper gameLooper,
            MapService mapService)
        {
            _dbContext = dbContext;
            _newTribeLocalizationInitializer = newTribeLocalizationInitializer;
            _gameLooper = gameLooper;
            _mapService = mapService;
        }


        public async Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId)
        {
            var tribe = await _dbContext
            .Tribes
            .Where(x => x.AccountId == accountId)
            .Select(x => new { x.Name, x.Id, x.Localization.X, x.Localization.Y })
            .SingleOrDefaultAsync()
                ?? throw new ArgumentException("No tribe founded in database with given accountId :/", nameof(accountId));

            var humanUnitsTask = _dbContext
                .HumanUnits
                .Where(x => x.TribeId == tribe.Id)
                .Select(x => new HumanUnitDto(x.Name, x.Localization.X, x.Localization.Y, x.FoodLevelPercentage))
                .ToListAsync();

            return new TribeGameObjectDto(new TribeDto(tribe.Id, tribe.Name, tribe.X, tribe.Y),
                await humanUnitsTask);
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
    }
}
