using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IMapTileRepository : IRepository
    {
        Task<List<MapTile>> GetByIds(List<int> ids);
    }

    internal class MapTileRepository : BaseRepository, IMapTileRepository 
    {
        public MapTileRepository(GameModuleDbContext gameModuleDbContext) : base(gameModuleDbContext)
        {
        }

        async Task<List<MapTile>> IMapTileRepository.GetByIds(List<int> ids)
        {
            return await _gameModuleDbContext
                .MapTiles
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
