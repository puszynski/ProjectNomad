namespace ProjectNomad.Shared
{
    public static class GameSETTINGS
    {
        public const int GAME_SPEED_FACTOR = 5;//for tests

        public const int MinutesToTravelOneTile = 10 * GAME_SPEED_FACTOR;

        public const double BasicProbabilityToAssignToTaskOrderPerSecond = 0.001 * GAME_SPEED_FACTOR;

        public static class Food
        {
            public const int MinutesToGatherFood = 10 / GAME_SPEED_FACTOR;

            public const int MinutesToConsumeFoodToFill20PercentageOfFood = 5 / GAME_SPEED_FACTOR;

            public const int FoodToGetHungryForHumanUnitEachMinute = 1 * GAME_SPEED_FACTOR;

            public const int TribeFoodNeededToFill20PercentageOfHumanUnit = 3;

            public const int MapTileFoodGathered = 3;
        }
        
        public static class MapResources
        {
            public const int FoodRegenerationPerHour = 10 * GAME_SPEED_FACTOR;
            public const int WoodRegenerationPerHour = 2 * GAME_SPEED_FACTOR;
        }

        public static class  Population
        {
            public const double BreedingChancePerHumanPerHour = 100;//0.02 * GAME_SPEED_FACTOR;//todo after tests retrive orginal factor
            public const double NaturalDeathChancePerHumanPerHour = 100;//0.01 * GAME_SPEED_FACTOR;//todo after tests retrive orginal factor

        }

        //todo sleep
    }
}
