using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    internal class MapViewModel
    {
        internal IEnumerable<MapTileViewModel> MapTiles { get; set; }
    }

    internal class MapTileViewModel : IMapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public EMapType Type { get; set; }
        public int FoodPoints { get; set; }
        public int WoodPoints { get; set; }
    }
}
