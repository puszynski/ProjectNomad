namespace ProjectNomad.Client.Logic
{
    // https://jasonwatmore.com/post/2020/07/30/aspnet-core-blazor-webassembly-communication-between-components
    // https://morioh.com/a/a5df9450ff5e/how-to-send-messages-between-components-in-a-blazor-webassembly-app
    public class EventBroadcastService
    {
        internal event Action<EActionWASM> OnAction; //if null, means that no one was subscribed to it
        internal void AddEvent(EActionWASM action)
        {
            OnAction?.Invoke(action);
        }

        internal void ClearEvent()
        {
            OnAction?.Invoke(EActionWASM.None);
        }
    }

    public enum EActionWASM //A MOŻE INFORMOWAĆ JAKI COMONENT CHCEMY ODŚWIEŻYC???
    {
        None = 0,

        NotificationAdded,
        TaskEnded,//e.g. TasksComponentCallForReload
        FoodGatheringStart //w.g MapCallForReload? 
    }
}