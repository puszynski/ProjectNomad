using Blazored.LocalStorage;
using System.Net.Http.Json;
using static ProjectNomad.Client.Components.Notifications;

namespace ProjectNomad.Client.Logic
{
    internal class TribeNotificationsLoclStorageAndServerManager
    {
        internal List<TribeNotificationDto> TribeNotifications = new List<TribeNotificationDto>();
        
        readonly ILocalStorageService _localStorageService;
        readonly HttpClient _httpClient;

        public TribeNotificationsLoclStorageAndServerManager(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        internal async Task GetAllAndRemoveOutdated(int tribeId)
        {
            //TODO NIE ODŚWIEŻASZ Z LOCAL STORAGE SERVICE - A MUSISZ BO MOZESZ DODAWAĆ W WASM..
            if (!TribeNotifications.Any())
            {
                var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<TribeNotificationDto>>("TribeNotifications");

                if (tribeNotificationsFromStorage is not null && tribeNotificationsFromStorage.Any())
                    TribeNotifications.AddRange(tribeNotificationsFromStorage);
            }

            var result = await _httpClient.GetFromJsonAsync<IEnumerable<TribeNotificationDto>>($"api/notification/get-notifications/{tribeId}");

            if (result is not null && result.Any())
            {
                TribeNotifications.AddRange(result);
                await _localStorageService.SetItemAsync<IEnumerable<TribeNotificationDto>>(
                    "TribeNotifications", 
                    TribeNotifications.OrderByDescending(x => x.Added));
            }
        }

        internal async Task Add(TribeNotificationDto dto, bool addToServer)
        {

            var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<TribeNotificationDto>>("TribeNotifications");

            if (tribeNotificationsFromStorage == null)
                tribeNotificationsFromStorage = new List<TribeNotificationDto>();
            else
                TribeNotifications = tribeNotificationsFromStorage.ToList();

            if (addToServer)
            {
                //todo to allow adding notifications to server from WASM (ID NEEDED.. SECIURITY?)
            }

            TribeNotifications.Add(dto);

            await _localStorageService.SetItemAsync<IEnumerable<TribeNotificationDto>>(
                "TribeNotifications",
                TribeNotifications.OrderByDescending(x => x.Added));
        }
    }
}
