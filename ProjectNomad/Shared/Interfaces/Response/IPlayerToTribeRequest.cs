namespace ProjectNomad.Shared.Interfaces.Response
{
    //idea

    //1. WASM - PlayerToTribeRequestManager.Get() and display actual PlayerToTribeRequest
    /// => displaying as progressBar(how much completed for a day) 
    /// 
    /// -> PlayerToTribeRequestManager.Add() new => LoadingBar + Server POST => saveInDb, as result new PlayerToTribeRequest => adding and display via progressBar
    //  -> PlayerToTribeRequestManager.Cancel() (via server) => LoadingBar + cancel POST => result -> remove
    //
    // GameLooper is checking actual PlayerToTribeRequest and its assigning tasks (+ sending notifications) about start/end of the task.. + display tasks in progress

    public interface IPlayerToTribeRequest
    {
        public int Id { get; set; } //needed id db => why? when if local storage will miss and all requests will disappear?! 
        public EPlayerToTribeRequestType Type { get; set; }
        public int? DesiredValue { get; set; } //e.g. 100 units of food
        public int? RepeatEachXDays { get; set; } //e.g. each 2 days, each 1... null - nor repeat
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public enum EPlayerToTribeRequestType
    {
        GatherFood,
        GatherWood
    }
}
