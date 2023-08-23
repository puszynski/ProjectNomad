namespace ProjectNomad.Shared.Interfaces.Response
{
    public interface ITriggerGameLooperResponse
    {
        IEnumerable<INotification> Notifications { get; set; }
        IEnumerable<IWorldEvent> WorldEvents { get; set; }
    }
}
