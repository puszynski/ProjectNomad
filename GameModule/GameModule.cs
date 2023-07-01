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
        public GameModule(GameModuleDbContext dbContext
            //NewTribeLocalizationInitializer newTribeLocalizationInitializer
            )
        {
            _dbContext = dbContext;
            //_newTribeLocalizationInitializer = newTribeLocalizationInitializer;
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

            return new TribeGameObjectDto(new TribeDto(tribe.Name, tribe.X, tribe.Y),
                await humanUnitsTask);
        }


        public async Task TrigerPlayerGameObjectRecalculation(Guid accountId)
        {
            throw new NotImplementedException();
        }


        public async Task InitPlayerGameObject(Guid accountId)
        {
            var initLocalization =  await _newTribeLocalizationInitializer.Initialize();

            var tribe = new Tribe()
            {
                AccountId = accountId,
                Name = "Tribe with no name",
                Localization = initLocalization
            };

            var humanUnits = new List<HumanUnit>()
            {
                new HumanUnit() { Name = "Aka", Localization = initLocalization, Tribe = tribe, FoodLevelPercentage = 100 },
                new HumanUnit() { Name = "Kha", Localization = initLocalization, Tribe = tribe, FoodLevelPercentage = 80 },
                new HumanUnit() { Name = "Buk", Localization = initLocalization, Tribe = tribe, FoodLevelPercentage = 70 },
                new HumanUnit() { Name = "Brio", Localization = initLocalization, Tribe = tribe, FoodLevelPercentage = 50 }
            };

            var taskTribe = _dbContext.AddAsync(tribe);
            var taskHumanUnits = _dbContext.AddAsync(humanUnits);

            await taskTribe;
            await taskHumanUnits;

            await _dbContext.SaveChangesAsync();
        }

    }
}
