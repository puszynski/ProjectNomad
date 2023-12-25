using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Logic;

namespace GameModule.Logic.Services
{
    internal class WeatherService //todo  - split ThermalService from WatherZoneService
    {
        private static int TEMPERATURE_DECRESE_IN_NIGHT = 10;

        readonly IWorldZoneParameterRepository _worldZoneParameterRepository;
        public WeatherService(IWorldZoneParameterRepository worldZoneParameterRepository)
        {
            _worldZoneParameterRepository = worldZoneParameterRepository;
        }

        internal void UpdateWorldZoneParameters(WorldZoneParameter worldZoneParameter)
        {
            worldZoneParameter.AverageTemperature = RandomCalculator.GetRandomInt(0, 40);
        }

        internal async Task<int> GetTemperatureFromZone(
            int averageTemperature, 
            DateTime now)
        {
            return DayNightService.IsNight(now) 
                ? averageTemperature - TEMPERATURE_DECRESE_IN_NIGHT 
                : averageTemperature;
        }
    }
}
