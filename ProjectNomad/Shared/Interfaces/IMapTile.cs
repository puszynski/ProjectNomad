using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    //  0,0 ---- X (100|X)
    //  |
    //  | Y (0 | 100)
    public interface IMapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public EMapType Type { get; set; }
        public int FoodPoints { get; set; }
    }
}