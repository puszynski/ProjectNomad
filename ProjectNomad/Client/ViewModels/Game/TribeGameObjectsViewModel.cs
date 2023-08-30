namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeGameObjectsViewModel //: ITribeGameObjects
    {
        public TribeViewModel Tribe { get; set; }
        public IEnumerable<HumanUnitViewModel> HumanUnits { get; set; }
    }
}