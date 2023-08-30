namespace ProjectNomad.Shared.Interfaces.Response
{
    public interface ITriggerGameLooperResponse
    {
        IEnumerable<INotification> Notifications { get; }
        IEnumerable<IWorldEvent> WorldEvents { get; }
    }
}
