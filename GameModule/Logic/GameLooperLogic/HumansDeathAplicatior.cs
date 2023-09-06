using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface IHumansDeathApplicator
    {
        List<INotification> StarvationDeath(List<HumanUnit> humanUnits);
    }

    internal class HumansDeathApplicator : IHumansDeathApplicator
    {
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IHumanUnitRepository _humanUnitRepository;
        public HumansDeathApplicator(IDateTimeProvider dateTimeProvider, 
            IHumanUnitRepository humanUnitRepository)
        {
            _dateTimeProvider = dateTimeProvider;
            _humanUnitRepository = humanUnitRepository;
        }

        List<INotification> IHumansDeathApplicator.StarvationDeath(List<HumanUnit> humanUnits)
        {
            List<INotification> notifications = new List<INotification>();
            var humansToDieFromStarving = humanUnits.Where(x => x.FoodLevelPercentage <= 0).ToList();

            if (humansToDieFromStarving.Any())
            {
                _humanUnitRepository.RemoveRange(humansToDieFromStarving);

                foreach (var human in humansToDieFromStarving)
                    notifications.Add(new NotificationDto(human.Id, 
                        human.Name, 
                        _dateTimeProvider.UtcNow(), 
                        ProjectNomad.Shared.Enums.ENotificationType.Starvation, 
                        null));
                
                foreach (var humanUnitRemoved in humansToDieFromStarving)
                        humanUnits.Remove(humanUnitRemoved);

            }           

            return notifications;
        }
    }
}
