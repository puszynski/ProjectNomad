using ProjectNomad.Client.ViewModels.Game;
using ProjectNomad.Client.ViewModels.Tasks;

namespace ProjectNomad.Client.DtoModels
{
    internal record TriggerGameLooperResponse(
        TribeViewModel Tribe,
        IEnumerable<HumanUnitViewModel> HumanUnits,
        IEnumerable<HumanUnitTaskViewModel> HumanUnitTasks,
        IEnumerable<HumanUnitTaskOrder> HumanUnitTaskOrders,
        IEnumerable<Notification> Notifications, 
        IEnumerable<WorldEvent> WorldEvents); //: ITriggerGameLooperResponse;
}
