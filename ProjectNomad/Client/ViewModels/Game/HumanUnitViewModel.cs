using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class HumanUnitViewModel : IHumanUnit
    {
        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int FoodLevelPercentage { get; set; }
    }
}
