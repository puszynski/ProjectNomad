using Microsoft.AspNetCore.Components;
using ProjectNomad.Client.Logic.GameLooperManagerLogic;
using ProjectNomad.Client.Models.Response;
using ProjectNomad.Shared.Interfaces;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class GameLooperManager
    {
        readonly LocalStorageNotificationsManager _notificationsManager;
        readonly EventBroadcastService _eventBroadcastService;
        readonly NavigationManager _navigationManager;
        readonly HttpClient _httpClient;

        public GameLooperManager(EventBroadcastService eventBroadcastService,
            LocalStorageNotificationsManager notificationsManager,
            HttpClient httpClient,
            NavigationManager navigationManager)
        {
            _eventBroadcastService = eventBroadcastService;
            _notificationsManager = notificationsManager;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        internal async Task<TriggerGameLooper> TriggerGameLooper(Guid accountId) 
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/game/triggerPlayerGameObjectRecalculation", accountId);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    //todo log
                    //+ co chcemy zrobić gdy coś się spypnie po stronie serwera??
                    _navigationManager.NavigateTo("error");
                    return null;
                }

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

                if (!responseContent.HumanUnits.Any())
                    _navigationManager.NavigateTo("gameOver");

                if (responseContent.Tribe.RelocationStatus == ETribeRelocationStatus.InProgress)
                    _navigationManager.NavigateTo($"/relocation");

                return responseContent;//UWAGA - NAWET JAK UŻYJESZ URL`I WYŻEJ - TJ gameOver/relocation/error - I TAK WRÓCISZ TUTAJ DO GameModule i będzie błąd..

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
