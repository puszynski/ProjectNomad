using ProjectNomad.Shared;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    internal class MapViewModel
    {
        internal IEnumerable<MapTileViewModel> MapTiles { get; set; }
        MapTileViewModel? SelectedMapTile { get; set; }
    }

    internal class MapTileViewModel : IMapTile
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
                case EMapType.RareConiferousForest:
                    imgName = "110_1";
                    break;

                    //random option now is disabled
                    //var randomValue = RandomCalculator.GetRandomInt(1, 3);
                    //if (randomValue == 3)
                    //{
                    //    imgName = "110_1";
                    //    break;
                    //}
                    //else if (randomValue == 2)
                    //{
                    //    imgName = "110_2";
                    //    break;
                    //}
                    //else
                    //{
                    //    imgName = "110_3";
                    //    break;
                    //}

                case EMapType.MediumConiferousForest:
                    imgName = "111_1";
                    break;

                case EMapType.DenseConiferousForest:
                    imgName = "112_1";
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
