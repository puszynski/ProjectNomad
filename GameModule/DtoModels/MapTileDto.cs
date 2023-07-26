using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record MapTileDto(int X, 
        int Y, 
        EMapType Type, 
        int FoodPoints,
        int WoodPoints) : IMapTile;
}
