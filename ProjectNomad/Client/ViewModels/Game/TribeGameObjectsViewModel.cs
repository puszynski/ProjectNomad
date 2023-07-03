using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeGameObjectsViewModel : ITribeGameObjects
    {
        public ITribe Tribe { get; set; }
        public IEnumerable<IHumanUnit> HumanUnits { get; set; }
    }
}
