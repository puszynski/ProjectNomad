namespace ProjectNomad.Shared.Logic
{
    public static class TaskDurationCalculator
    {
        public static TimeSpan TribeRelocation(int mapTileDistance)
            => TimeSpan.FromMinutes(mapTileDistance * GameSETTINGS.Moving.MinutesToTravelOneTileWhileTribeIsRelocating);

        public static TimeSpan WoodGathering(int mapTileDistance)
            => TimeSpan.FromMinutes(2 * mapTileDistance * GameSETTINGS.Moving.MinutesToTravelOneTile + GameSETTINGS.Wood.MinutesToGatherWood);
    }
}
