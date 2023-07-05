using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeViewModel : ITribe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
