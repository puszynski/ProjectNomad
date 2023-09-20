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

        async Task<List<MapTile>> IMapTileRepository.GetByLocalizations(List<Localization> localizations)
        {
            //var t1 = new Localization() { X = 1, Y = 2 };
            //var t2 = new Localization() { X = 1, Y = 2 };
            //var t00 = t1.GetHashCode();
            //var t01 = t2.GetHashCode();
            //var tt = t1 == t2; // == underneath its equal for classes
            //var tt2 = t1.Equals(t2); //works becouse of eq override

            //var t_list = new List<Localization>() { t1, t2 };
            //var t_eq = t_list.Where(x => x.Equals(t1)).ToList();

            //var t_list2 = new List<Localization>() { t1, t2 };

            //var common_elements = t_list2.Where(x => t_list.Contains(x)); 

            
            
            //var t444 = await _gameModuleDbContext
            //        .MapTiles
            //        .Where(x => x.Localization.Equals(t1))
            //        .ToListAsync(); //No backing field could be found for property pTile.Localization#Localization.MapTileId' and the property does not have a etter


            //T E M P   S O L U T I O N  - DO IT IN PROP WAY..
            var result = new List<MapTile>();
            foreach (var localization in localizations)
            {
                var localizationEqual = await _gameModuleDbContext
                .MapTiles
                .SingleOrDefaultAsync(x => localization.X == x.Localization.X 
                && localization.Y==x.Localization.Y);

                if (localizationEqual != null)
                    result.Add(localizationEqual);
            }
            return result;
            //return await _gameModuleDbContext
            //    .MapTiles
            //    .Where(x => localizations.Contains(x.Localization))
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
            var taskOrder = await _gameModuleDbContext.HumanUnitTaskOrders.SingleAsync(x => x.Id == taskOrderId);
            _gameModuleDbContext.Remove(taskOrder);
        }
    }
}
