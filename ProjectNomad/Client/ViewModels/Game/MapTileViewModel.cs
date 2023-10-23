using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    internal class MapViewModel
    {
        internal IEnumerable<MapTileViewModel> MapTiles { get; set; }
        MapTileViewModel? SelectedMapTile { get; set; }
    }

    public class MapTileViewModel : IMapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public EMapType Type { get; set; }
        public int FoodPoints { get; set; }
        public int WoodPoints { get; set; }


        internal string GetTileImgPath()
        {
            string imgName;

            switch (Type)
            {
                case EMapType.Forest:
                    imgName = WoodPoints < 33 ? "110_1c" : WoodPoints < 66 ? "111b" : "112b";
                    break;

                case EMapType.Ocean:
                default:
                    imgName = "0_1";
                    break;
            }

            return $"/images/game/map/{imgName}.png";
        }
    }
}
