namespace ProjectNomad.Shared
{
    public static class GameSETTINGS
    {
        public const int GAME_SPEED_FACTOR = 5;//FOR TEST PURPUSE..



        public const int MinutesToTravelOneTile = 10 * GAME_SPEED_FACTOR;

        public const double ProbabilityToAssignToTaskOrder = 0.01 * GAME_SPEED_FACTOR;

        public const int MinutesToGatherFood = 10 / GAME_SPEED_FACTOR;
        public const int MinutesToConsumeFoodToFill20PercentageOfFood = 5 / GAME_SPEED_FACTOR;
        public const int FoodToGetHungryForHumanUnitEachMinute = 1 * GAME_SPEED_FACTOR;
        public const int TribeFoodNeededToFill20PercentageOfHumanUnit = 3 * GAME_SPEED_FACTOR;

        //todo sleep
    }
}
