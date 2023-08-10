namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnit
    {
        public int Id { get; }
        string Name { get; }
        int X { get; }
        int Y { get; }
        int FoodLevelPercentage { get; }
    }
}