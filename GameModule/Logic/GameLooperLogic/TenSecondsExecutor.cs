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
        readonly IHumansDeathApplicator _humansDeathApplicator;
        readonly HumanThermalService _humanThermalService;
        readonly WeatherService _weatherService;
        public TenSecondsExecutor(
            IHumansDeathApplicator humansDeathApplicator,
            HumanThermalService humanThermalService,
            WeatherService weatherService)
        {
            _humansDeathApplicator = humansDeathApplicator;
            _humanThermalService = humanThermalService;
            _weatherService = weatherService;
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

            var temperature = await _weatherService.GetTemperatureFromZone(worldZoneParameter.AverageTemperature, currentTimeInLoop);
            _humanThermalService.UpdateHumanThermalLevel(tribe, temperature);

            _humansDeathApplicator.FreezeDeath(tribe.Humans, notifications);
            _humansDeathApplicator.OverheatingDeath(tribe.Humans, notifications);
        }
    }
}
