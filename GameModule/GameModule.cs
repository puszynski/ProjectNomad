using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Entities.ValueObjects;
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
        public GameModule(GameModuleDbContext dbContext,
            NewTribeLocalizationInitializer newTribeLocalizationInitializer,
            GameLooper gameLooper)
        {
            _dbContext = dbContext;
            _newTribeLocalizationInitializer = newTribeLocalizationInitializer;
            _gameLooper = gameLooper;
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


        public async Task InitPlayerGameObject(Guid accountId)
        {
            var localization =  await _newTribeLocalizationInitializer.Initialize();

            var tribe = new Tribe()
            {
                AccountId = accountId,
                Name = "Tribe with no name",
                Localization = localization,
                Updated = DateTime.UtcNow
            };

            var humanUnits = new List<HumanUnit>()
            {
                new HumanUnit() { Name = "Aka", Localization = new Localization { X = 1, Y = 1 }, Tribe = tribe, FoodLevelPercentage = 100 },
                new HumanUnit() { Name = "Kha", Localization = new Localization { X = 1, Y = 1 }, Tribe = tribe, FoodLevelPercentage = 80 },
                new HumanUnit() { Name = "Buk", Localization = new Localization { X = 1, Y = 1 }, Tribe = tribe, FoodLevelPercentage = 70 },
                new HumanUnit() { Name = "Brio", Localization = new Localization { X = 1, Y = 1 }, Tribe = tribe, FoodLevelPercentage = 50 }
            };

            try
            {
                var taskTribe = _dbContext.Tribes.AddAsync(tribe);
                await taskTribe;
                await _dbContext.SaveChangesAsync();

                var taskHumanUnits = _dbContext.HumanUnits.AddRangeAsync(humanUnits);//why localization is getting null?
                //SqlException: Cannot insert the value NULL into column 'Localization_X', table 'ProjectNomad.GameModule.HumanUnits'; column does not allow nulls. UPDATE fails.
                await taskHumanUnits;
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
