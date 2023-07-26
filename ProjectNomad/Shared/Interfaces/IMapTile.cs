using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    //  0,0 ---- X (100|X)
    //  |
    //  | Y (0 | 100)
    public interface IMapTile
    {
        public int X { get; }
        public int Y { get; }
        public EMapType Type { get; }
        public int FoodPoints { get; }
        public int WoodPoints { get; }
    }
}