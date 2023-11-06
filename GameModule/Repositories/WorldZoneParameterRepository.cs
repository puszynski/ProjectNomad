using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IWorldZoneParameterRepository : IRepository
    {
        internal Task<WorldZoneParameter> GetByZone(EWorldZoneParameter zone);
        internal Task<WorldZoneParameter> Create();
    }
    internal class WorldZoneParameterRepository : BaseRepository, IWorldZoneParameterRepository
    {
        public WorldZoneParameterRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<WorldZoneParameter?> IWorldZoneParameterRepository.GetByZone(EWorldZoneParameter zone)
        {
            return await _gameModuleDbContext
                .WorldZoneParameter
                .SingleOrDefaultAsync(x => x.Zone == zone);
        }

        async Task<WorldZoneParameter> IWorldZoneParameterRepository.Create()
        {
            //todo validate if not exist and create set for all zones
            var model = new WorldZoneParameter
            {
                Zone = EWorldZoneParameter.Temperate,
                AverageTemperature = 10,
                IsBlizzard = false,
                IsRain = false,
                IsSnow = false,
                IsWind = false
            };

            await _gameModuleDbContext.WorldZoneParameter.AddAsync(model);
            return model;
        }
    }
}
