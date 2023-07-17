using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    internal class GameModule : IGameModule
    {
        private readonly GameModuleDbContext _dbContext;
        private readonly NewTribeLocalizationInitializer _newTribeLocalizationInitializer;
        private readonly GameLooper _gameLooper;
        private readonly MapService _mapService;
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
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task TriggerPlayerGameObjectRecalculation(int tribeId) 
            => _gameLooper.LoopTribe(tribeId);


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

            try
            {
                var taskTribe = _dbContext.Tribes.AddAsync(tribe);
                await taskTribe;//if save below is not neede, move await down
                //await _dbContext.SaveChangesAsync();//needed?

                var taskHumanUnits = _dbContext.HumanUnits.AddRangeAsync(humanUnits);
                await taskHumanUnits;
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task InitPlayerGameObjectsForExistingTribe(int tribeId)
        {
            var tribe = await _dbContext
                .Tribes
                .SingleOrDefaultAsync(x => x.Id == tribeId);

            if (tribe == null)
                throw new ArgumentException(nameof(tribe), $"No tribe for given id ({tribeId}) existing :/");

            if (await _dbContext.HumanUnits.AnyAsync(x => x.TribeId == tribe.Id))
                throw new ArgumentException($"To init new human units, there should be no in database (tribeId:{tribe.Id}) :/");

            var humanUnits = new List<HumanUnit>();

            for (int i = 0; i < 4; i++)
                humanUnits.Add(HumanUnitGenerator.Generate(tribe));

            await _dbContext.HumanUnits.AddRangeAsync(humanUnits);
            await _dbContext.SaveChangesAsync();
        }


        public Task<IEnumerable<IMapTile>> GetMapData(int x, int y)
        {
            return _mapService.GetMapData(x, y); 
        }
    }
}
