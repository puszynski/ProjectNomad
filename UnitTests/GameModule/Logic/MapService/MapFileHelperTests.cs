using GameModule.Logic.MapServiceLogic;
using Xunit;

namespace UnitTests.GameModule.Logic.MapService
{
    internal class MapFileHelperTests
    {
        [Theory]
        [InlineData(2, 2, "0_0-99_99")]
        [InlineData(50, 50, "0_0-99_99")]
        public void Should_return_one_map_name(int x, int y, string mapName)
        {
            var result = MapFileHelper.GetMapFilesNames(x, y);
        }
    }
}
