using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanUnitDto(string Name, 
        int X, 
        int Y, 
        int FoodLevelPercentage) : IHumanUnit;
}
