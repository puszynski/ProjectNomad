using Blazored.LocalStorage;
using ProjectNomad.Client.Logic.NotificationsManagerLogic;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Response;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class GameLooperManager
    {
        const string LOCAL_STORAGE_WORLD_EVENTS_NAME = "WorldEvents";

        readonly NotificationsManager _notificationsManager;
        readonly ILocalStorageService _localStorageService;
        readonly HttpClient _httpClient;

        internal GameLooperManager(NotificationsManager notificationsManager,
            ILocalStorageService localStorageService,
            HttpClient httpClient)
        {
            _notificationsManager = notificationsManager;
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        internal async Task TriggerGameLooper(Guid accountId) 
        {
            //todo trigger
            var response = await _httpClient.PostAsJsonAsync("api/game/triggerPlayerGameObjectRecalculation", accountId); //TODO RESPONSE FROM SERVER.. - NOTIFIFCATIONS + GAME EVENTS
            var responseContent = await response.Content.ReadFromJsonAsync<ITriggerGameLooperResponse>();

            //fill _localStorageService via _notificationsManager with server notifications
            if (responseContent?.Notifications != null)
                foreach (var notificationToAdd in responseContent.Notifications)
                    await _notificationsManager.Add(notificationToAdd.HumanUnitId, notificationToAdd.Type, notificationToAdd.CustomValue);

            //fill WorldEvents _localStorageService with worldEvents
            if (responseContent?.Notifications != null)
                foreach (var notificationToRemove in responseContent.WorldEvents)
                    throw new NotImplementedException();
                    //await _worldEventsManager.Add();

        }   

        internal async Task AddNotificationFromWASM(int humanUnitId, 
            ENotificationType type, 
            string? customValue)
        {
            if (!NotificationTypeValidator.IsWASMNotification(type))
                throw new ArgumentException($"Ops WASM! GameLooperManager.AddNotificationFromWASM() can receive only WASM notification, {type} is server one :/");

            await _notificationsManager.Add(humanUnitId, type, customValue);
        }

        internal async Task<IEnumerable<Notification>> GetNotifications() 
            => await _notificationsManager.Get();

        internal async Task<IEnumerable<Object>> GetWorldEvents()
        {
            throw new NotImplementedException();
        }
    }
}
