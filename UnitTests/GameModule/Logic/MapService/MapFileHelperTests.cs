using GameModule.Logic.MapServiceLogic;
using Xunit;

namespace UnitTests.GameModule.Logic.MapService
{
    public class MapFileHelperTests
    {
        [Theory]
        [InlineData(3, 3, "0_0-99_99")]
        [InlineData(96, 96, "0_0-99_99")]

        [InlineData(103, 3, "100_0-199_99")]
        [InlineData(196, 96, "100_0-199_99")]

        [InlineData(3, 103, "0_100-99_199")]
        [InlineData(96, 196, "0_100-99_199")]
        public void Should_return_one_map_name(int x, int y, string mapName)
        {
            var result = MapFileHelper.GetMapFilesNames(x, y);

            Assert.Single(result);
            Assert.Equal(mapName, result.Single());
        }
    }
}
