using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace GameModule.Logic.Services
{
    internal class MapService
    {
        internal async Task<EMapType> GetRandomMapTile()
        {
            var randomNumber = RandomCalculator.GetRandomInt(1, 10);

            switch (randomNumber)
            {
                case >= 10: return EMapType.Mountains;
                default: return EMapType.Forest;
            }
        }

        internal async Task<TileRecourse> GetRandomValuesForWoodTileResource(EMapType type)
        {
            if (type is not EMapType.Forest)
                return new TileRecourse { ActualPoints = 0, MaxLimitPoints = 0 };

            var maxWoodPoints = RandomCalculator.GetRandomIntWithHigherProbabilityOfLowerValues();
            return new TileRecourse { ActualPoints = maxWoodPoints, MaxLimitPoints = maxWoodPoints };
        }

        internal async Task<IEnumerable<MapTile>> GenerateMapTiles(int x_start,
            int y_start)
        {
            if (x_start % 100 != 0 || y_start % 100 != 0)
                throw new ArgumentOutOfRangeException("X and Y should point to the stating position of the map(right-top corner) :/");


            var tiles = new List<MapTile>();

            for (int x = x_start; x < x_start + 100; x++)
            {
                for (int y = y_start; y < y_start + 100; y++)
                {
                    var localization = new Localization { X = x, Y = y };

                    var type = await GetRandomMapTile();

                    var maxFoodPoints = RandomCalculator.GetRandomIntWithHigherProbabilityOfLowerValues();
                    var food = new TileRecourse { ActualPoints = maxFoodPoints, MaxLimitPoints = maxFoodPoints };

                    var wood = await GetRandomValuesForWoodTileResource(type);

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
