using GameModule.Configurations;
using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared;

namespace GameModule.Logic.Services
{
    internal class NewTribeLocalizationInitializer
    {
        private readonly GameModuleDbContext _dbContext;
        public NewTribeLocalizationInitializer(GameModuleDbContext dbContext)
            => _dbContext = dbContext;

        internal async Task<Localization> Initialize()
        {
            var localization = new Localization() { X = 3, Y = 3 };

            if (await _dbContext.Tribes.AnyAsync())
            {
                try
                {
                    while (_dbContext.Tribes.Any(x => x.Localization.X == localization.X && x.Localization.Y == localization.Y))
                    {
                        //note - simple fast solution - should be updated in the future
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
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return localization;
        }
    }
}
