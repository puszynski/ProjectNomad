using GameModule.DtoModels;
using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.SharedExecutorLogic
{
    internal interface IHumansDeathApplicator
    {
        void StarvationDeath(ICollection<Human> humanUnits, List<INotification> notifications);
        void FreezeDeath(ICollection<Human> humanUnits, List<INotification> notifications);
        void OverheatingDeath(ICollection<Human> humanUnits, List<INotification> notifications);
        void AgeOrIllnessDeath(ICollection<Human> humanUnits, List<INotification> notifications);
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

        void IHumansDeathApplicator.StarvationDeath(ICollection<Human> humanUnits, List<INotification> notifications)
        {
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
        }

        void IHumansDeathApplicator.FreezeDeath(ICollection<Human> humans, List<INotification> notifications)
        {
            var humansToDieFromStarving = humans.Where(x => x.ThermalLevelPercentage <= 0).ToList();

            if (humansToDieFromStarving.Any())
            {
                _humanUnitRepository.RemoveRange(humansToDieFromStarving);

                foreach (var human in humansToDieFromStarving)
                    notifications.Add(new NotificationDto(human.Id,
                        human.Name,
                        _dateTimeProvider.UtcNow(),
                        ProjectNomad.Shared.Enums.ENotificationType.DeathFromFreezing,
                        null));

                foreach (var humanUnitRemoved in humansToDieFromStarving)
                    humans.Remove(humanUnitRemoved);
            }

            if (!humans.Any())
            {
                var tests = "testuje sobie gdzie idzie wyjątek..";
            }
        }

        void IHumansDeathApplicator.OverheatingDeath(ICollection<Human> humans, List<INotification> notifications)
        {
            var humansToDieFromStarving = humans.Where(x => x.ThermalLevelPercentage >= 100).ToList();

            if (humansToDieFromStarving.Any())
            {
                _humanUnitRepository.RemoveRange(humansToDieFromStarving);

                foreach (var human in humansToDieFromStarving)
                    notifications.Add(new NotificationDto(human.Id,
                        human.Name,
                        _dateTimeProvider.UtcNow(),
                        ProjectNomad.Shared.Enums.ENotificationType.DeathFromOverheat,
                        null));

                foreach (var humanUnitRemoved in humansToDieFromStarving)
                    humans.Remove(humanUnitRemoved);

            }
        }

        void IHumansDeathApplicator.AgeOrIllnessDeath(ICollection<Human> humanUnits, List<INotification> notifications)
        {
            if (!RandomCalculator.GetBoolWithGivenProbability(humanUnits.Count() * GameSETTINGS.Population.NaturalDeathChancePerHumanPerHour))
                return;

            var human = humanUnits.First();
            humanUnits.Remove(human);

            notifications.Add(new NotificationDto(human.Id,
                    human.Name,
                    _dateTimeProvider.UtcNow(),
                    ProjectNomad.Shared.Enums.ENotificationType.DeathFromAgeOrIllness,
                    null));
        }
    }
}
