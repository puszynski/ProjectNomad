using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class HumanUnitViewModel : IHumanUnit
    {
        public int Id { get; }
        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int FoodLevelPercentage { get; set; }
    }
}
