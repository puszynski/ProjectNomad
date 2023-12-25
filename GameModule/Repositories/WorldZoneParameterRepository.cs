using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IWorldZoneParameterRepository : IRepository
    {
        internal Task<WorldZoneParameter> GetByZone(EWorldZoneParameter zone);
        internal Task<WorldZoneParameter> Create(EWorldZoneParameter zone);
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

        async Task<WorldZoneParameter> IWorldZoneParameterRepository.Create(EWorldZoneParameter zone)
        {
            var existingZone = await _gameModuleDbContext
                .WorldZoneParameter
                .SingleOrDefaultAsync(x => x.Zone == zone);

            if (existingZone != null)
                throw new ArgumentException($"(!) WorldZoneParameter for zone {zone} can not be created because it already exists in database");

            var model = new WorldZoneParameter
            {
                Zone = zone,
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
