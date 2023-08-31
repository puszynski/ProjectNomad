using ProjectNomad.Client.DtoModels;
using ProjectNomad.Client.Logic.GameLooperManagerLogic;
using ProjectNomad.Shared.Enums;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class GameLooperManager
    {
        const string LOCAL_STORAGE_WORLD_EVENTS_NAME = "WorldEvents";

        //todo LocalStorageWorldEventsManager
        readonly LocalStorageNotificationsManager _notificationsManager;
        readonly HttpClient _httpClient;

        public GameLooperManager(LocalStorageNotificationsManager notificationsManager,
            HttpClient httpClient)
        {
            _notificationsManager = notificationsManager;
            _httpClient = httpClient;
        }

        internal async Task TriggerGameLooper(Guid accountId) 
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/game/triggerPlayerGameObjectRecalculation", accountId);
                var responseContent = await response.Content.ReadFromJsonAsync<TriggerGameLooperResponse>(); 
                //'Deserialization of interface types is not supported. Type 'ProjectNomad.Shared.Interfaces.INotification'. Path: $.notifications[0] | LineNumber: 0 | BytePositionInLine: 19.'

                if (responseContent?.Notifications != null)
                    foreach (var notificationToAdd in responseContent.Notifications)
                        await _notificationsManager.Add(notificationToAdd.HumanUnitId, 
                            notificationToAdd.HumanUnitName, 
                            notificationToAdd.Type, 
                            notificationToAdd.CustomValue);

                //fill WorldEvents _localStorageService with worldEvents
                if (responseContent?.Notifications != null)
                    foreach (var notificationToRemove in responseContent.WorldEvents)
                        throw new NotImplementedException();
                //await _worldEventsManager.Add();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }   

        internal async Task AddNotificationFromWASM(int humanUnitId, 
            ENotificationType type, 
            string? customValue)
        {
            if (!NotificationTypeValidator.IsWASMNotification(type))
                throw new ArgumentException($"Ops WASM! GameLooperManager.AddNotificationFromWASM() can receive only WASM notification, {type} is server one :/");

            await _notificationsManager.Add(humanUnitId, "unknowWasmTodo", type, customValue);
        }

        internal async Task<IEnumerable<Notification>> GetNotifications() 
            => await _notificationsManager.Get();

        internal async Task<IEnumerable<Object>> GetWorldEvents()
        {
            throw new NotImplementedException();
        }
    }
}
