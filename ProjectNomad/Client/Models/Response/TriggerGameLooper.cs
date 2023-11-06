using ProjectNomad.Shared.DTOs.ServerToWasm;

namespace ProjectNomad.Client.Models.Response
{
    internal record TriggerGameLooper(
        Tribe Tribe,
        IEnumerable<HumanUnit> HumanUnits,
        IEnumerable<HumanUnitTask> HumanUnitTasks,
        IEnumerable<HumanUnitTaskOrder> HumanUnitTaskOrders,
        IEnumerable<TribeStructure> TribeStructures,
        IEnumerable<Notification> Notifications,
        WorldParametersDto WorldParameters); //: ITriggerGameLooperResponse;
}
