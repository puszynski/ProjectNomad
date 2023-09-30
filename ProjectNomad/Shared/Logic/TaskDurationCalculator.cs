namespace ProjectNomad.Shared.Logic
{
    public static class TaskDurationCalculator
    {
        public static TimeSpan TribeRelocation(int mapTileDistance)
            => TimeSpan.FromMinutes(mapTileDistance * GameSETTINGS.Moving.MinutesToTravelOneTileWhileTribeIsRelocating);
    }
}
