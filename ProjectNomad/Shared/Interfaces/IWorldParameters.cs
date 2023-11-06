namespace ProjectNomad.Shared.Interfaces
{
    public interface IWorldParameters
    {
        public int AverageTemperature { get; }
        public bool IsWind { get; }
        public bool IsRain { get; }
        public bool IsSnow { get; }
        public bool IsBlizzard { get; }
    }
}
