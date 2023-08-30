using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace GameModule.DtoModels
{
    internal record TriggerGameLooperResponse(IEnumerable<INotification> Notifications,
        IEnumerable<IWorldEvent> WorldEvents) : ITriggerGameLooperResponse;

}
