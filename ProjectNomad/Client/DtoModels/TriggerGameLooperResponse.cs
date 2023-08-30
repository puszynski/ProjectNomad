using ProjectNomad.Shared.Interfaces.Response;

namespace ProjectNomad.Client.DtoModels
{
    internal record TriggerGameLooperResponse(
        IEnumerable<Notification> Notifications, 
        IEnumerable<WorldEvent> WorldEvents); //: ITriggerGameLooperResponse;
}
