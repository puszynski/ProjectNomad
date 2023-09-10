using ProjectNomad.Client.ViewModels.Game;

namespace ProjectNomad.Client.DtoModels
{
    internal record TriggerGameLooperResponse(
        TribeViewModel Tribe,
        IEnumerable<HumanUnitViewModel> HumanUnits,
        IEnumerable<Notification> Notifications, 
        IEnumerable<WorldEvent> WorldEvents); //: ITriggerGameLooperResponse;
}
