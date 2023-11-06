using ProjectNomad.Shared.DTOs.ServerToWasm;

namespace GameModule.Entities
{
    internal class WorldZoneParameter
    {
        public int Id { get; set; }
        public EWorldZoneParameter Zone { get; set; }  //one entity per Zone
        public int AverageTemperature { get; set; }
        public bool IsWind { get; set; }
        public bool IsRain { get; set; }
        public bool IsSnow { get; set; }
        public bool IsBlizzard { get; set; }

        public static implicit operator WorldParametersDto(WorldZoneParameter model) 
            => new WorldParametersDto(model.AverageTemperature, model.IsWind, model.IsRain, model.IsSnow, model.IsBlizzard);
    }

    internal enum EWorldZoneParameter
    {
        None = 0,

        //world climates zones:
        Polar = 1,
        Subpolar = 2,
        Temperate = 3,
        Subtropical = 4,
        Tropical = 5,
        Equatorial = 6,

        //EUROPE: tundra, subarctic, marine, highland, humid continental, cold semi-arid, marine, and Mediterranean
    }
}
