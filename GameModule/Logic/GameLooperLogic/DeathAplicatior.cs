using GameModule.Configurations;
using GameModule.DtoModels;
using GameModule.Entities;
using NotificationModule;
using ProjectNomad.Shared.Enums;

namespace GameModule.Logic.GameLooperLogic
{
    internal static class DeathApplicator
    {
        internal static void StarvationDeath(GameModuleDbContext _dbContext,
            INotificationModule notificationModule,
            ICollection<HumanUnit> humanUnits)
        {
            var humansToDieFromStarving = humanUnits.Where(x => x.FoodLevelPercentage <= 0);

            if (humansToDieFromStarving.Any())
            {
                _dbContext.RemoveRange(humansToDieFromStarving);

                humansToDieFromStarving.ToList()
                    .ForEach(x => CreateNotification(x, notificationModule));
            }
        }

        static void CreateNotification(HumanUnit humanUnit, INotificationModule notificationModule)
        {
            var tribeNotificationDto = new TribeNotificationDto(humanUnit.Id, 
                DateTime.UtcNow, 
                EHumanNotificationType.DeathFromStarvation);

            notificationModule.InsertTribeNotification(humanUnit.TribeId, tribeNotificationDto);
        }
    }
}
