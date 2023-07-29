using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace GameModule.Logic
{
    internal class MapService
    {
        internal async Task<EMapType> GetRandomMapTile()
        {
            var randomNumber = RandomCalculator.GetRandomInt(1, 10);

            switch (randomNumber)
            {
                case >= 10: return EMapType.DenseConiferousForest;
                case >= 6: return EMapType.MediumConiferousForest;
                default: return EMapType.RareConiferousForest;
            }
        }

        internal async Task<TileRecourse> GetRandomValuesForWoodTileResource(EMapType type, int tileWoodPointsMaxLimit)
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
