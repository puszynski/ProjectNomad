namespace ProjectNomad.Shared.Interfaces.Response
{
    public interface ITriggerGameLooperResponse
    {
        ITribe Tribe { get; }
        IEnumerable<IHumanUnit> HumanUnits { get; }
        IEnumerable<ITribeStructure> TribeStructures { get; }
        IEnumerable<IHumanUnitTaskDto> HumanUnitTasks { get; }
        IEnumerable<IHumanUnitTaskOrder> HumanUnitTaskOrders { get; }
        IEnumerable<INotification> Notifications { get; }
        IEnumerable<IWorldEvent> WorldEvents { get; }
    }
}
