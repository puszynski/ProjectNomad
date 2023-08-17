using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Logic.GameLooperLogic;
using Microsoft.EntityFrameworkCore;
using NotificationModule;

namespace GameModule.Logic
{
    internal class GameLOOPER
    {
        readonly GameModuleDbContext _dbContext;
        readonly INotificationModule _notificationModule;
        readonly HumanUnitTaskConsumer _humanUnitTaskConsumer;
        public GameLOOPER(GameModuleDbContext dbContext, 
            INotificationModule notificationModule, 
            HumanUnitTaskConsumer humanUnitTaskConsumer)
        {
            _dbContext = dbContext;
            _notificationModule = notificationModule;
            _humanUnitTaskConsumer = humanUnitTaskConsumer;
        }
        async Task SecondLooper(List<HumanUnit> humanUnits, 
            Tribe tribe, 
            IEnumerable<HumanUnitTask> tasksToConsume)//IT TAKES A LOT TIME... TODO - DTO WITH ALL DATA NEEDED TO CONSUME TASK EG MAP-TILE
        {
            await _humanUnitTaskConsumer.Execute(humanUnits, tribe, tasksToConsume);
        }

        async Task MinuteLooper(List<HumanUnit> humanUnits, Tribe tribe)
        {
            humanUnits.ForEach(x => x.FoodLevelPercentage--);
            DeathApplicator.StarvationDeath(_dbContext, _notificationModule, humanUnits);

            humanUnits.Where(x => x.FoodLevelPercentage == 69).ToList()
                .ForEach(x => _notificationModule.InsertTribeNotification(x.TribeId, new TribeNotificationDto(x.Id, DateTime.UtcNow, ProjectNomad.Shared.Enums.EHumanNotificationType.FoodHunger)));

            humanUnits.Where(x => x.FoodLevelPercentage == 29).ToList()
                .ForEach(x => _notificationModule.InsertTribeNotification(x.TribeId, new TribeNotificationDto(x.Id, DateTime.UtcNow, ProjectNomad.Shared.Enums.EHumanNotificationType.FoodStarvation)));
        }

        async Task HourLooper(List<HumanUnit> humanUnits)
        {
            await Console.Out.WriteLineAsync("HourLooper is running");
        }

        async Task DayLooper(List<HumanUnit> humanUnits) 
        {
            await Console.Out.WriteLineAsync("DayLooper is running");
        }

        public async Task LoopTribe(int tribeId)
        {
            //todo try async query materializations.. - there were problems..

            var tribe = _dbContext
                .Tribes
                .SingleOrDefault(x => x.Id == tribeId);

            var humanUnits = _dbContext
                .HumanUnits
                .Where(x => x.TribeId == tribeId)
                .ToList();

            var tasksToConsume = _dbContext.HumanUnitTasks
                .Where(x => x.TribeId == tribeId)
                .ToList();

            if (tribe == null || !humanUnits.Any())
                return;

            var timeToUpdate = tribe.Updated;
            var loopCounter = 0;

            while (timeToUpdate <= DateTime.UtcNow)
            {
                await SecondLooper(humanUnits, tribe, tasksToConsume);

                if (timeToUpdate.Second == 0)
                    await MinuteLooper(humanUnits, tribe);
                if (timeToUpdate.Minute == 0)
                    await HourLooper(humanUnits);
                if (timeToUpdate.Hour == 0)
                    await DayLooper(humanUnits);

                timeToUpdate = timeToUpdate.AddSeconds(1);
                loopCounter++;

            }

            if (loopCounter != 0)
            {
                tribe.Updated = DateTime.UtcNow;
                _dbContext.SaveChanges();
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
