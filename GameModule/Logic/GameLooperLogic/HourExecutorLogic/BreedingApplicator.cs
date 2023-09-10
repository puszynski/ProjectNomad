using GameModule.DtoModels;
using GameModule.Entities;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic.HourExecutorLogic
{
    internal class BreedingApplicator
    {
        readonly IDateTimeProvider _dateTimeProvider;
        public BreedingApplicator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        internal void Execute(Tribe tribe, List<INotification> notifications)
        {
            var humansReadyToMateCount = tribe.HumanUnits.Where(x => x.FoodLevelPercentage >= 90).Count();

            if (humansReadyToMateCount < 2)
                return;

            if (RandomCalculator.GetBoolWithGivenProbability(humansReadyToMateCount * GameSETTINGS.Population.BreedingChancePerHumanPerHour))
            {
                var newborn = HumanUnitGenerator.Generate(tribe);
                tribe.HumanUnits.Add(newborn);

                notifications.Add(new NotificationDto(newborn.Id,
                        newborn.Name,
                        _dateTimeProvider.UtcNow(),
                        ProjectNomad.Shared.Enums.ENotificationType.Newborn,
                        null));
            }
        }
    }
}
