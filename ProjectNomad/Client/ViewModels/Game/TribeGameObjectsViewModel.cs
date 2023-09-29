using ProjectNomad.Client.DtoModels;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeGameObjectsViewModel //: ITribeGameObjects
    {
        public TribeViewModel Tribe { get; set; }
        public IEnumerable<HumanUnitViewModel> HumanUnits { get; set; }
        public IEnumerable<IHumanUnitTaskDto> HumanUnitTasks { get; set; }
        public IEnumerable<HumanUnitTaskOrder> HumanUnitTaskOrders { get; set; }
    }
}