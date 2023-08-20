namespace ProjectNomad.Shared
{
    public static class HumanUnitSpeedCalculator
    {
        public static TimeSpan CalculateTravelSpeed(int distance, int foodLevelPercentage)
        {
            var humanUnitTravelSpeed = GameSETTINGS.MinutesToTravelOneTile * ((double)foodLevelPercentage / 100);
            return TimeSpan.FromMinutes(humanUnitTravelSpeed * distance);
        }
    }
}
