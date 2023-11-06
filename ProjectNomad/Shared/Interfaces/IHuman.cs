namespace ProjectNomad.Shared.Interfaces
{
    public interface IHuman
    {
        public int Id { get; }
        string Name { get; }
        int X { get; }
        int Y { get; }
        int FoodLevelPercentage { get; }
        int ThermalLevelPercentage { get; }
    }
}