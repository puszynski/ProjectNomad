using GameModule.Configurations;
using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace GameModule.Logic
{
    internal class NewTribeLocalizationInitializer
    {
        private readonly GameModuleDbContext _dbContext;

        internal NewTribeLocalizationInitializer(GameModuleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        internal async Task<Localization> Initialize()
        {
            var localization = new Localization() { X = 0, Y = 0 };            

            while (_dbContext.Tribes.Any(x => x.Localization.Equals(localization)))
            {
                //note - simple fast solution to be updated in the future
                var maxX_Task = _dbContext.Tribes.Select(x => x.Localization).MaxAsync(x => x.X);
                var maxY_Task = _dbContext.Tribes.Select(x => x.Localization).MaxAsync(x => x.X);

                var maxX = await maxX_Task;
                var maxY = await maxY_Task;

                localization = new Localization 
                { 
                    X = maxX + RandomCalculator.GetRandomInt(1, 3), 
                    Y = maxY + RandomCalculator.GetRandomInt(1, 3) 
                };
            }

            return localization;
        }
    }
}
