using ProjectNomad.Shared.DTOs.ServerToWasm;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace GameModule.DtoModels
{
    internal record TriggerGameLooperResponse(ITribe Tribe,
        IEnumerable<IHuman> HumanUnits,
        IEnumerable<IHumanUnitTaskDto> HumanUnitTasks,
        IEnumerable<IHumanUnitTaskOrder> HumanUnitTaskOrders,
        IEnumerable<ITribeStructure> TribeStructures,
        IEnumerable<INotification> Notifications,
        WorldParametersDto WorldParameters) : ITriggerGameLooperResponse;
}
