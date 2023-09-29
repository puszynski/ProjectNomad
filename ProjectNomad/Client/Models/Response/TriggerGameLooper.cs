namespace ProjectNomad.Client.Models.Response
{
    internal record TriggerGameLooper(
        Tribe Tribe,
        IEnumerable<HumanUnit> HumanUnits,
        IEnumerable<HumanUnitTask> HumanUnitTasks,
        IEnumerable<HumanUnitTaskOrder> HumanUnitTaskOrders,
        IEnumerable<Notification> Notifications,
        IEnumerable<WorldEvent> WorldEvents); //: ITriggerGameLooperResponse;
}
