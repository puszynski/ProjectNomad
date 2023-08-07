using ProjectNomad.Shared;
using Xunit;

namespace UnitTests.ProjectNomadSharedTests
{
    public class MapTileDistanceCalculatorTests
    {
        [Theory]
        [InlineData(0,0, 1,1, 2)]
        [InlineData(0,0, 5,5, 10)]
        public void ManhattanDistance_should_return_expectedValues(int startX, int startY, int endX,  int endY, int expectedValue)
        {
            var resultAsDefault = MapTileDistanceCalculator.Execute(startX, startY, endX, endY);
            var result = MapTileDistanceCalculator.Execute(startX, startY, endX, endY, EDistanceCalculationAlgorithm.Manhattan);

            Assert.Equal(expectedValue, resultAsDefault);
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(0, 0, 1, 1, 1)]
        [InlineData(0, 0, 5, 7, 7)]
        public void Chebyshev_should_return_expectedValues(int startX, int startY, int endX, int endY, int expectedValue)
        {
            var result = MapTileDistanceCalculator.Execute(startX, startY, endX, endY, EDistanceCalculationAlgorithm.Chebyshev);

            Assert.Equal(expectedValue, result);
        }
    }
}
