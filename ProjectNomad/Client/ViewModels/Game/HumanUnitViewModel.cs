using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class HumanUnitViewModel : IHumanUnit
    {
        public string Name { get; }

        public int X { get; }

        public int Y { get; }

        public int FoodLevelPercentage { get; }
    }
}
