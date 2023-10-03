namespace ProjectNomad.Shared
{
    public static class GameSETTINGS
    {
        public const double BasicProbabilityToAssignToTaskOrderPerSecond = 0.01;

        public static class Moving
        {
            public const int MinutesToTravelOneTile = 10;
            public const int MinutesToTravelOneTileWhileTribeIsRelocating = 1; // temp for test
            //public const int MinutesToTravelOneTileWhileTribeIsRelocating = 30;
        }

        public static class Wood
        {
            public const int MinutesToGatherWood = 10;
            public const int WoodAmountGatheredFromMap = 2;
        }

        public static class Food
        {
            public const int MinutesToGatherFood = 10;
            public const int MinutesToConsumeFoodToFill20PercentageOfFood = 2;
            public const int FoodToGetHungryForHumanUnitEachMinute = 1;
            public const int TribeFoodNeededToFill20PercentageOfHumanUnit = 2;
            public const int MapTileFoodGathered = 7;
        }

        public static class Fire
        {
            public const int TimeToCompleteAttemptToStartFire = 1;
            public const double ChanceToStartFire = 0.1;

            public const int WoodUsedToKeepTheCampfireBurning = 1;
            public const int WoodUsedToKeepTheBonfireBurning = 5;

            public const int PowerAndDurabilityForCampfire = 20;
            public const int PowerAndDurabilityForBonfire = 100;


            public const int MinutesOfFireDuration = 1; // JAKIŚ NOWY BYT - TYPU TribeItem? *to będzie paleniko, budynki..
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
