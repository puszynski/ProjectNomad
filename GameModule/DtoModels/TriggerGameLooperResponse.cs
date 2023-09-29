using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace GameModule.DtoModels
{
    internal record TriggerGameLooperResponse(ITribe Tribe,
        IEnumerable<IHumanUnit> HumanUnits,
        IEnumerable<IHumanUnitTaskDto> HumanUnitTasks,
        IEnumerable<IHumanUnitTaskOrder> HumanUnitTaskOrders,
        IEnumerable<INotification> Notifications,
        IEnumerable<IWorldEvent> WorldEvents) : ITriggerGameLooperResponse;
}
