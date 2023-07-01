using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeGameObjectsViewModel : ITribeGameObjects
    {
        public TribeGameObjectsViewModel(ITribe tribe, 
            IEnumerable<IHumanUnit> humanUnits)
        {
            Tribe = tribe;
            HumanUnits = humanUnits;
        }

        public ITribe Tribe { get; }
        public IEnumerable<IHumanUnit> HumanUnits { get; }
    }
}
