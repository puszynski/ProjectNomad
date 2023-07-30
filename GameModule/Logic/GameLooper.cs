using GameModule.Configurations;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using Microsoft.EntityFrameworkCore;
using NotificationModule;

namespace GameModule.Logic
{
    internal class GameLooper
    {
        readonly GameModuleDbContext _dbContext;
        readonly INotificationModule _notificationModule;
        public GameLooper(GameModuleDbContext dbContext, INotificationModule notificationModule)
        {
            _dbContext = dbContext;
            _notificationModule = notificationModule;
        }

        void MinuteLooper(List<HumanUnit> humanUnits)
        {
            humanUnits.ForEach(x => x.FoodLevelPercentage--);
            var humanUnitsRemoved = DeathApplicator.StarvationDeath(_dbContext, _notificationModule, humanUnits);

            if (humanUnitsRemoved != null)
                foreach (var humanUnitRemoved in humanUnitsRemoved)
                    humanUnits.Remove(humanUnitRemoved);
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
                var tribe = _dbContext
                    .Tribes
                    .SingleOrDefault(x => x.Id == tribeId);

                //var tribe = await _dbContext //PROBLEM WITH ASYNC.. TODO IN FUTURE 
                //  .Tribes
                //  .SingleOrDefaultAsync(x => x.Id == tribeId);

                var humanUnits = _dbContext
                    .HumanUnits
                    .Where(x => x.TribeId == tribeId)
                    .ToList();

                if (tribe == null || !humanUnits.Any())
                    return;

                var timeToUpdate = tribe.Updated;
                var loopCounter = 0;

                while (timeToUpdate <= DateTime.UtcNow.AddMinutes(-1)) //BUG if you refresh frequently - no change after 1 min.. 
                {
                    MinuteLooper(humanUnits);
                    if (timeToUpdate.Minute == 0)
                        HourLooper(humanUnits);
                    if (timeToUpdate.Hour == 0)
                        DayLooper(humanUnits);

                    timeToUpdate = timeToUpdate.AddMinutes(1);
                    loopCounter++;

                }

                if (loopCounter != 0)
                {
                    tribe.Updated = DateTime.UtcNow;
                    _dbContext.SaveChanges();
                }
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
