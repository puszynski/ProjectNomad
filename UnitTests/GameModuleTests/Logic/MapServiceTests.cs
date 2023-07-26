using Xunit;

namespace UnitTests.GameModuleTests.Logic
{
    public class MapServiceTests
    {
        [Fact]
        public async Task GenerateMapTiles_should_generate_random_wood_and_food_values()
        {
            var mapService = new GameModule.Logic.MapService();

            var TILE_MAX_FOOD_POINTS_LIMIT = 100;
            var TILE_MAX_WOOD_POINTS_LIMIT = 100;

            var tiles = await mapService.GenerateMapTiles(0,
                0,
                TILE_MAX_FOOD_POINTS_LIMIT,
                TILE_MAX_WOOD_POINTS_LIMIT);

            var woodAverage = tiles.Select(x => x.Wood.ActualPoints).Average();
            var foodAverage = tiles.Select(x => x.Food.ActualPoints).Average();


            Assert.True(woodAverage != 0);
            Assert.True(woodAverage != 100);
            Assert.True(foodAverage != 0);
            Assert.True(foodAverage != 100);

        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(99, 99)]
        [InlineData(101, 101)]
        public async Task GenerateMapTiles_should_throw_ArgumentOutOfRangeException_when_x_y_do_not_point_to_start_of_square(int x, int y)
        {
            var mapService = new GameModule.Logic.MapService();

            var TILE_MAX_FOOD_POINTS_LIMIT = 100;
            var TILE_MAX_WOOD_POINTS_LIMIT = 100;

            Func<Task> testCode = () => mapService.GenerateMapTiles(x,
                y,
                TILE_MAX_FOOD_POINTS_LIMIT,
                TILE_MAX_WOOD_POINTS_LIMIT);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(testCode);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 0)]
        [InlineData(100, 100)]
        public async Task GenerateMapTiles_should_generate_100x100_tiles(int x, int y)
        {
            var mapService = new GameModule.Logic.MapService();

            var TILE_MAX_FOOD_POINTS_LIMIT = 100;
            var TILE_MAX_WOOD_POINTS_LIMIT = 100;

            var tiles = await mapService.GenerateMapTiles(x,
                y,
                TILE_MAX_FOOD_POINTS_LIMIT,
                TILE_MAX_WOOD_POINTS_LIMIT);

            Assert.Equal(100*100, tiles.Count());

            Assert.True(tiles.Where(t => t.Localization.X == x && t.Localization.Y == y).Count() == 1);
            Assert.True(tiles.Where(t => t.Localization.X == x + 99 && t.Localization.Y == y + 99).Count() == 1);

            Assert.True(tiles.Min(t => t.Localization.X) == x);
            Assert.True(tiles.Min(t => t.Localization.Y) == y);

            Assert.True(tiles.Max(t => t.Localization.X) == x + 99);
            Assert.True(tiles.Max(t => t.Localization.Y) == y + 99);

        }
    }
}
