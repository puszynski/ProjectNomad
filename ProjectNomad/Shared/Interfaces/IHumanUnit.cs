namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnit
    {
        public string Name { get; }
        public int X { get; }
        public int Y { get; }
        public int FoodLevelPercentage { get; }
    }
}
