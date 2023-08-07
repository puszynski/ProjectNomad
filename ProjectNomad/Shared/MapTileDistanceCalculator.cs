namespace ProjectNomad.Shared
{
    public enum EDistanceCalculationAlgorithm
    {
        Manhattan = 0,
        Chebyshev = 1,
        Euclidean = 2
    }

    public static class MapTileDistanceCalculator
    {       
        public static int Execute(int startX, 
            int startY, 
            int endX, 
            int endY, 
            EDistanceCalculationAlgorithm strategy = EDistanceCalculationAlgorithm.Manhattan)
        {
            switch (strategy)
            {
                case EDistanceCalculationAlgorithm.Chebyshev:
                    return Chebyshev(startX, startY, endX, endY);

                case EDistanceCalculationAlgorithm.Euclidean:
                    return Euclidean(startX, startY, endX, endY);

                default:
                    return ManhattanDistance(startX, startY, endX, endY); 
            }
        }

        // X 1 2 3
        // 1 2 3 4
        // 2 3 4 5
        // 3 4 5 6
        static int ManhattanDistance(int startX, int startY, int endX, int endY) 
            => Math.Abs(startX - endX) + Math.Abs(startY - endY);

        // X 1 2 3
        // 1 1 2 3
        // 2 2 2 3
        // 3 3 3 3
        static int Chebyshev(int startX, int startY, int endX, int endY)
            => Math.Max(Math.Abs(startX - endX), Math.Abs(startY - endY));

        // note(!) - Euclidean algorithm returns double - calculator is rounding it to int
        // x    1       2
        // 1    1.42    2.24
        // 2    2.24    2.83
        static int Euclidean(int startX, int startY, int endX, int endY)
            => (int)Math.Sqrt(Math.Pow((double)startX - endX, 2) + Math.Pow((double)startY - endY, 2));
    }
}
