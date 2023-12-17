using GameModule.Entities;
using GameModule.Logic.GameLooperLogic.SharedExecutorLogic;
using GameModule.Logic.GameLooperLogic.TasksLogic;
using GameModule.Logic.Services;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Logic.GameLooperLogic
{
    internal interface ITenSecondsExecutor
    {
        Task Execute(Tribe tribe,
            WorldZoneParameter worldZoneParameter,
            List<INotification> notifications,
            DateTime currentTimeInLoop);
    }

    internal class TenSecondsExecutor : ITenSecondsExecutor
    {
        readonly WeatherAndThermalService _weatherAndThermalService;
        readonly IHumansDeathApplicator _humansDeathApplicator;
        public TenSecondsExecutor(WeatherAndThermalService weatherAndThermalService, 
            IHumansDeathApplicator humansDeathApplicator)
        {
            _weatherAndThermalService = weatherAndThermalService;
            _humansDeathApplicator = humansDeathApplicator;
        }

        public async Task Execute(Tribe tribe,
            WorldZoneParameter worldZoneParameter, 
            List<INotification> notifications, 
            DateTime currentTimeInLoop)
        {
            var campfire = tribe
                .TribeStructures?
                .SingleOrDefault(x => x.Type == ProjectNomad.Shared.Enums.ETribeStructureType.Firecamp);
            Campfire.CampfireBurning(campfire);

            _weatherAndThermalService.UpdateHumanThermalLevel(tribe, worldZoneParameter.AverageTemperature);

            _humansDeathApplicator.FreezeDeath(tribe.Humans, notifications);
            _humansDeathApplicator.OverheatingDeath(tribe.Humans, notifications);
        }
    }
}
