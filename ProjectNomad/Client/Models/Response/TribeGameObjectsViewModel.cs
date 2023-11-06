using ProjectNomad.Shared.DTOs.ServerToWasm;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public class TribeGameObjectsViewModel //: ITribeGameObjects
    {
        public Tribe Tribe { get; set; }
        public IEnumerable<HumanUnit> HumanUnits { get; set; }
        public IEnumerable<IHumanUnitTaskDto> HumanUnitTasks { get; set; }
        public IEnumerable<HumanUnitTaskOrder> HumanUnitTaskOrders { get; set; }
        public IEnumerable<ITribeStructure> TribeStructures { get; set; }
        public WorldParametersDto WorldParameters { get; set; }
    }
}