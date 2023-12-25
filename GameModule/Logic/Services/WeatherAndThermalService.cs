using GameModule.Entities;
using GameModule.Repositories;
using ProjectNomad.Shared;

namespace GameModule.Logic.Services
{
    internal class WeatherAndThermalService //todo  - split ThermalService from WatherZoneService
    {
        readonly IWorldZoneParameterRepository _worldZoneParameterRepository;
        public WeatherAndThermalService(IWorldZoneParameterRepository worldZoneParameterRepository)
        {
            _worldZoneParameterRepository = worldZoneParameterRepository;
        }

        /// <summary>
        /// NOTE: when human is freezing, he can auto-assign heatNearFire auto task
        /// </summary>
        internal void UpdateHumanThermalLevel(Tribe tribe, int temperature)
        {
            if (tribe.Humans == null)
                return;

            if (temperature < -10)
                tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage -= 3);
            else if (temperature < 0)
                tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage -= 2);
            else if (temperature < 10)
                tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage--);

            else if (temperature > 35)
                tribe.Humans.ToList().ForEach(x => x.ThermalLevelPercentage++);

            else //optimal(10-35)
            {
                tribe.Humans.Where(x => x.ThermalLevelPercentage < 50).ToList().ForEach(x => x.ThermalLevelPercentage++);
                tribe.Humans.Where(x => x.ThermalLevelPercentage > 50).ToList().ForEach(x => x.ThermalLevelPercentage--);
            }
        }

        internal void UpdateWorldZoneParameters(WorldZoneParameter worldZoneParameter)
        {
            //todo sth more..
            worldZoneParameter.AverageTemperature = RandomCalculator.GetRandomInt(0, 40);
        }

        internal async Task<int> GetTemperatureFromZone(Tribe tribe)
        {
            //todo - cache... 
            //todo - other zone base on tribe localization...
            var zoneParameters = await _worldZoneParameterRepository.GetByZone(EWorldZoneParameter.Temperate);

            if (zoneParameters == null)
                throw new ArgumentNullException($"(!) WorldZoneParameter does not exist in database for zone {EWorldZoneParameter.Temperate}");

            return zoneParameters.AverageTemperature;
        }
    }
}
