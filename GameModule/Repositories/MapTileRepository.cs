using GameModule.Configurations;
using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IMapTileRepository : IRepository
    {
        Task<List<MapTile>> GetByIds(List<int> ids);
        Task<int> GetIdByLocalization(int x, int y);
        Task Remove(int taskOrderId);
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

        async Task<int> IMapTileRepository.GetIdByLocalization(int x, int y)
        {
            return await _gameModuleDbContext
                .MapTiles
                .Where(mt => mt.Localization.X == x && mt.Localization.Y == y)
                .Select(mt => mt.Id)
                .SingleAsync();
        }

        async Task IMapTileRepository.Remove(int taskOrderId)
        {
            var taskOrder = await _gameModuleDbContext.HumanUnitTaskOrders.SingleAsync(x => x.Id == taskOrderId);
            _gameModuleDbContext.Remove(taskOrder);
        }
    }
}
