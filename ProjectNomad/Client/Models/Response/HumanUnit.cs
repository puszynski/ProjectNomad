using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record HumanUnit(int Id,
        string Name,
        int X,
        int Y,
        int FoodLevelPercentage) : IHumanUnit;
}
