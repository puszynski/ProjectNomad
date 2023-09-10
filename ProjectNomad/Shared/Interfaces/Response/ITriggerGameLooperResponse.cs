namespace ProjectNomad.Shared.Interfaces.Response
{
    public interface ITriggerGameLooperResponse
    {
        ITribe Tribe { get; }
        IEnumerable<IHumanUnit> HumanUnits { get; }
        IEnumerable<INotification> Notifications { get; }
        IEnumerable<IWorldEvent> WorldEvents { get; }
    }
}
