namespace ProjectNomad.Shared
{
    public static class RandomCalculator
    {
        /// <param name="probability">probability range from 0.01 - 1</param>
        public static bool GetBoolWithGivenProbability(double probability = 0.5)
        {
            var random = new Random();
            var randomDouble = random.NextDouble();
            return randomDouble < probability;
        }

        public static int GetRandomInt(int min = 1, int max = 100)
        {
            var random = new Random();
            return random.Next(min, max + 1);
        }
    }
}
