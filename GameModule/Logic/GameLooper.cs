using GameModule.Configurations;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Logic
{
    internal class GameLooper
    {
        private readonly GameModuleDbContext _dbContext;
        public GameLooper(GameModuleDbContext dbContext) 
            => _dbContext = dbContext;

        void MinuteLooper(List<HumanUnit> humanUnits)
        {
            DeathApplicator.StarvationDeath(_dbContext, humanUnits);
            humanUnits.ForEach(x => x.FoodLevelPercentage--);
        }

        void HourLooper(List<HumanUnit> humanUnits)
        {

        }

        void DayLooper(List<HumanUnit> humanUnits) 
        { 
        
        }

        public async Task LoopTribe(int tribeId)
        {
            try
            {
                //todo GAME OVER - WHAT TO DO WHEN ALL HUMANS ARE DEAD? DISPLAY INFO AND ALLOW TO START AGAIN? 


                var tribe = _dbContext
                    .Tribes
                    .SingleOrDefault(x => x.Id == tribeId);

                //var tribe = await _dbContext //problem with async.. 
                //.Tribes
                //.SingleOrDefaultAsync(x => x.Id == tribeId);

                var humanUnits = _dbContext
                    .HumanUnits
                    .Where(x => x.TribeId == tribeId)
                    .ToList();

                if (tribe == null || !humanUnits.Any())
                    return;

                var timeToUpdate = tribe.Updated;

                while (timeToUpdate <= DateTime.UtcNow)
                {
                    MinuteLooper(humanUnits);
                    if (timeToUpdate.Minute == 0)
                        HourLooper(humanUnits);
                    if (timeToUpdate.Hour == 0)
                        DayLooper(humanUnits);

                    timeToUpdate = timeToUpdate.AddMinutes(1);
                }

                tribe.Updated = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //idea - do in night task? - or in restart server time once per night?
        public async Task LoopAll()
        {
            //todo check AccountLastLogIn - disable old Tribes
            var allTribesIds = await _dbContext
                .Tribes
                .Select(x => x.Id)
                .ToListAsync();

            allTribesIds.ForEach(x => LoopTribe(x));
        }
    }
}
