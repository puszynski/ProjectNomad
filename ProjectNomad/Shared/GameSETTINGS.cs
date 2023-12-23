namespace ProjectNomad.Shared
{
    public static class GameSETTINGS
    {
        public const double BasicProbabilityToAssignToTaskOrderPerSecond = 0.05;

        public static class InitializeRebornTribe
        {
            public const int FoodPoints = 20;
            public const int WoodPoints = 20;
        }

        public static class Moving
        {
            public const int MinutesToTravelOneTile = 10;
            public const int MinutesToTravelOneTileWhileTribeIsRelocating = 1; // temp for test
            //public const int MinutesToTravelOneTileWhileTribeIsRelocating = 30;
        }

        public static class HumanConditions
        {
            public const int CriticalFoodLevel = 20;
            public const int CriticalLowThermalLevel = 20;
            public const int CriticalHighThermalLevel = 80;
        }

        public static class Wood
        {
            public const int MinutesToGatherWood = 1;
            public const int WoodAmountGatheredFromMap = 5;
        }

        public static class Food
        {
            public const int MinutesToGatherFood = 5;
            public const int MapTileFoodGathered = 2;

            public const int MinutesToConsumeFoodToFill20PercentageOfFood = 1;
            public const int FoodToGetHungryForHumanUnitEachMinute = 1;
            public const int TribeFoodNeededToFill20PercentageOfHumanUnit = 2;
        }

        public static class Fire
        {
            public const int TimeToCompleteFireUp = 1;
            public const double ChanceToStartFire = 0.2;

            public const int WoodUsedToKeepTheCampfireBurning = 1;
            public const int BurningCampFireEch10Seconds = 1;

            public const int TimeToHeatUpHumanByFire = 1; //todo depends on lvl, if freezeing, very long to recover..
        }

        public static class MapResources
        {
            public const int FoodRegenerationPerHour = 10;
            public const int WoodRegenerationPerHour = 2;
        }

        public static class  Population
        {
            public const double BreedingChancePerHumanPerHour = 0.02;
            public const double NaturalDeathChancePerHumanPerHour = 0.01;
        }

        public static class TribeRelocation
        {
            public const int FoodPointsNeededToTravelOneTileForOneTribeMember = 1;
        }
    }
}
