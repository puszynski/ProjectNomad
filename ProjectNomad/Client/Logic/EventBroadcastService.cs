namespace ProjectNomad.Client.Logic
{
    // https://morioh.com/a/a5df9450ff5e/how-to-send-messages-between-components-in-a-blazor-webassembly-app
    public class EventBroadcastService
    {
        internal event Action<EActionWASM> OnAction;
        internal void AddMessage(EActionWASM action)
        {
            OnAction.Invoke(action);
        }

        internal void ClearEvent()
        {
            OnAction.Invoke(EActionWASM.None);
        }
    }

    public enum EActionWASM
    {
        None = 0,

        NotificationAdded = 1,
    }
}
