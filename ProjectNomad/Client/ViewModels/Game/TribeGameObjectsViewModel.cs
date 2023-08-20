using System.Net.Http.Json;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeGameObjectsViewModel
    {
        public TribeViewModel Tribe { get; set; }
        public IEnumerable<HumanUnitViewModel> HumanUnits { get; set; }
    }
}