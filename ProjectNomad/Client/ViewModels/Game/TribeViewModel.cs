using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeViewModel : ITribe
    {
        public string Name { get; }

        public int X { get; }

        public int Y { get; }
    }
}
