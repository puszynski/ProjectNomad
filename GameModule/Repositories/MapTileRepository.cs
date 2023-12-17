using GameModule.Configurations;
using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Repositories
{
    internal interface IMapTileRepository : IRepository
    {
        Task<List<MapTile>> GetByLocalizations(List<Localization> localizations);
        Task<List<MapTile>> GetByIds(List<int> ids);
        IEnumerable<MapTile> GetQueryFromCondition(
            Func<MapTile, bool> condition1,
            Func<MapTile, bool> condition2 = null);
        //IQueryable<MapTile> GetQueryFromRadius(Localization startLocalization, int radius);
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

        IEnumerable<MapTile> IMapTileRepository.GetQueryFromCondition(
            Func<MapTile, bool> condition1, 
            Func<MapTile, bool> condition2 = null)
        {
            var query = _gameModuleDbContext
                .MapTiles
                .Where(condition1);

            if (condition2 != null)
                query.Where(condition2);

            return query;
        }

        //IQueryable<MapTile> IMapTileRepository.GetQueryFromRadius(
        //    Localization startLocalization, 
        //    int radius)
        //{
        //    return _gameModuleDbContext
        //        .MapTiles
        //        .Where(x => x.Localization.X >= startLocalization.X - radius
        //            && x.Localization.X <= startLocalization.X + radius
        //            && x.Localization.Y >= startLocalization.Y - radius
        //            && x.Localization.Y <= startLocalization.Y + radius);
        //}

        async Task<List<MapTile>> IMapTileRepository.GetByLocalizations(List<Localization> localizations)
        {
            var result = new List<MapTile>();
            foreach (var localization in localizations)
            {
                var localizationEqual = await _gameModuleDbContext
                    .MapTiles
                    .SingleOrDefaultAsync(mt => localization.X == mt.Localization.X 
                                             && localization.Y == mt.Localization.Y);

                if (localizationEqual != null)
                    result.Add(localizationEqual);
            }
            return result;
            //return await _gameModuleDbContext
            //    .MapTiles
            //    .Where(x => localizations.Contains(x.Localization)) //not working, same as equal in linq...
            //    .ToListAsync();
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
            var taskOrder = await _gameModuleDbContext.HumanTaskOrders.SingleAsync(x => x.Id == taskOrderId);
            _gameModuleDbContext.Remove(taskOrder);
        }
    }
}
