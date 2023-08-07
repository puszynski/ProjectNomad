using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Shared
{
    public static class HumanUnitSpeedCalculator
    {
        public static TimeSpan CalculateTravelSpeed(int distance, int foodLevelPercentage)
        {
            const int TIME_TRAVEL_ONE_TILE_IN_MINUTES = 10;

            var humanUnitTravelSpeed = TIME_TRAVEL_ONE_TILE_IN_MINUTES * ((double)foodLevelPercentage / 100);

            return TimeSpan.FromMinutes(humanUnitTravelSpeed * distance);
        }
    }
}
