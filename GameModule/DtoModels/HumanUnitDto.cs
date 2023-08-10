using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanUnitDto(int Id,
        string Name, 
        int X, 
        int Y, 
        int FoodLevelPercentage) : IHumanUnit;
}
