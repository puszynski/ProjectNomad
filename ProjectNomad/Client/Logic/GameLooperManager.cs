using ProjectNomad.Client.Logic.GameLooperManagerLogic;
using ProjectNomad.Client.Models.Response;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class GameLooperManager
    {
        readonly LocalStorageNotificationsManager _notificationsManager;
        readonly EventBroadcastService _eventBroadcastService;
        readonly HttpClient _httpClient;

        public GameLooperManager(EventBroadcastService eventBroadcastService,
            LocalStorageNotificationsManager notificationsManager,
            HttpClient httpClient)
        {
            _eventBroadcastService = eventBroadcastService;
            _notificationsManager = notificationsManager;
            _httpClient = httpClient;
        }

        internal async Task<TriggerGameLooper> TriggerGameLooper(Guid accountId) 
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/game/triggerPlayerGameObjectRecalculation", accountId);
                var responseContent = await response.Content.ReadFromJsonAsync<TriggerGameLooper>(); 
                //RelocationStatus is not mapping  correctly.. 

                if (responseContent?.Notifications?.Any() == true)
                {
                    foreach (var notificationToAdd in responseContent.Notifications)
                        await _notificationsManager.Add(notificationToAdd.HumanUnitId, 
                            notificationToAdd.HumanUnitName, 
                            notificationToAdd.Type, 
                            notificationToAdd.CustomValue);

                    _eventBroadcastService.AddEvent(EActionWASM.NotificationAdded); //chyba się jeszcze utworzyły i nie zasuskrybowały inne componenty..
                }

                return responseContent;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   


        internal async Task<IEnumerable<Notification>> GetNotifications() 
            => await _notificationsManager.Get();

        internal async Task<IEnumerable<Object>> GetWorldEvents() 
            => throw new NotImplementedException();
    }
}
