using ProjectNomad.Shared;
using Xunit;

namespace UnitTests.ProjectNomadSharedTests
{
    public class HumanUnitSpeedCalculatorTests
    {
        [Theory]
        [InlineData(10, 100, 100)]
        [InlineData(10, 50, 50)]
        [InlineData(10, 0, 0)]
        public void Test(int distance, int humanUnitFoodLevel, int expectedResultInMinutes)
        {
            var result = HumanUnitSpeedCalculator.CalculateTravelSpeed(distance, humanUnitFoodLevel);
            Assert.Equal(TimeSpan.FromMinutes(expectedResultInMinutes), result);
        }
    }
}
