namespace ProjectNomad.Client.Logic
{
    // https://jasonwatmore.com/post/2020/07/30/aspnet-core-blazor-webassembly-communication-between-components
    // https://morioh.com/a/a5df9450ff5e/how-to-send-messages-between-components-in-a-blazor-webassembly-app
    public class EventBroadcastService
    {
        internal event Action<EActionWASM, string> OnAction; //if null, means that no one was subscribed to it

        internal void AddEvent(EActionWASM action, string value) 
            => OnAction?.Invoke(action, value);

        internal void AddEvent(EActionWASM action) 
            => OnAction?.Invoke(action, string.Empty);

        internal void ClearEvent() 
            => OnAction?.Invoke(EActionWASM.None, string.Empty);
    }

    public enum EActionWASM //A MOŻE INFORMOWAĆ JAKI COMONENT CHCEMY ODŚWIEŻYC???
    {
        None = 0,

        NotificationAdded,
        TaskEnded,//e.g. TasksComponentCallForReload
        FoodGatheringStart, //w.g MapCallForReload? 

        MapTileSelected,
    }
}