using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace GameModule.Logic
{
    internal class MapService
    {
        internal async Task<IEnumerable<IMapTile>> GetMapData(int x, int y)
        {
            if (x < 3 && y < 3)
                throw new NotImplementedException();
            


            throw new NotImplementedException();

            //todo - get from db after refactor
            //var fileNames = MapFileHelper.GetMapFilesNames(x, y);

            //var allMapTilesNeeded = new List<IMapTile>();

            //foreach ( var fileName in fileNames)
            //{
            //    var filePath = MapDirectoryPathName(fileName);
            //    FileStream fileStream = new FileStream(filePath, FileMode.Open);
            //    using (StreamReader reader = new StreamReader(fileStream))
            //    {
            //        string line = await reader.ReadToEndAsync();
            //        var dataFromFiles = JsonSerializer.Deserialize<IEnumerable<IMapTile>>(line);
            //        allMapTilesNeeded.AddRange(dataFromFiles); 
            //    }
            //}

            throw new NotImplementedException(); //todo return 7x7 
        }

        internal async Task<EMapType> GetRandomMapTile()
        {
            var randomNumber = RandomCalculator.GetRandomInt(1, 10);

            switch (randomNumber)
            {
                case 10: return EMapType.DenseConiferousForest;
                case 6 - 9: return EMapType.MediumConiferousForest;
                default: return EMapType.RareConiferousForest;
            }
        }

        internal async Task<TileRecourse> GetRandomValuesForWoodTileResource(EMapType type, int tileWoodPointsMaxLimit) //todo nei działa - UT
        {
            if (type is not EMapType.DenseConiferousForest and not EMapType.MediumConiferousForest and not EMapType.RareConiferousForest)
                return new TileRecourse { ActualPoints = 0, MaxLimitPoints = 0 };

            var maxWoodPoints = 0;

            if (type is EMapType.DenseConiferousForest)
                maxWoodPoints = RandomCalculator.GetRandomInt(tileWoodPointsMaxLimit / 3 * 2, tileWoodPointsMaxLimit);
            else if (type is EMapType.MediumConiferousForest)
                maxWoodPoints = RandomCalculator.GetRandomInt(tileWoodPointsMaxLimit / 3, tileWoodPointsMaxLimit / 3 * 2);
            else
                maxWoodPoints = RandomCalculator.GetRandomInt(tileWoodPointsMaxLimit / 4, tileWoodPointsMaxLimit / 3);

            return new TileRecourse { ActualPoints = maxWoodPoints, MaxLimitPoints = maxWoodPoints };
        }

        internal async Task<IEnumerable<MapTile>> GenerateMapTiles(int x_start, 
            int y_start, 
            int TILE_MAX_FOOD_POINTS_LIMIT, 
            int TILE_MAX_WOOD_POINTS_LIMIT)
        {
            if (x_start % 100 != 0 || y_start % 100 != 0)
                throw new ArgumentOutOfRangeException("X and Y should point to the stating position of the map(right-top corner) :/");


            var tiles = new List<MapTile>();

            for (int x = x_start; x < x_start + 100; x++)
            {
                for (int y = y_start; y < y_start + 100; y++)
                {
                    var localization = new Localization { X = x, Y = y };

                    var maxFoodPoints = RandomCalculator.GetRandomInt(0, TILE_MAX_FOOD_POINTS_LIMIT);
                    var food = new TileRecourse { ActualPoints = maxFoodPoints, MaxLimitPoints = maxFoodPoints };

                    var type = await GetRandomMapTile();
                    var wood = await GetRandomValuesForWoodTileResource(type, TILE_MAX_WOOD_POINTS_LIMIT);

                    var tile = new MapTile()
                    {
                        Localization = localization,
                        Food = food,
                        Wood = wood,
                        Type = type
                    };

                    tiles.Add(tile);
                }
            }

            return tiles;
        }
    }
}
