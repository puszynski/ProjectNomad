namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnit
    {
        string Name { get; }
        int X { get; }
        int Y { get; }
        int FoodLevelPercentage { get; }
    }
}