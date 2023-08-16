using Blazored.LocalStorage;
using System.Net.Http.Json;
using static ProjectNomad.Client.Components.Notifications;

namespace ProjectNomad.Client.Logic
{
    internal class TribeNotificationsLocalStorageAndServerManager
    {
        internal List<TribeNotificationDto> TribeNotifications;
        
        readonly ILocalStorageService _localStorageService;
        readonly HttpClient _httpClient;

        public TribeNotificationsLocalStorageAndServerManager(ILocalStorageService localStorageService, 
            HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        internal async Task GetAllAndRemoveOutdated(int tribeId)
        {
            TribeNotifications = new List<TribeNotificationDto>();

            var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<TribeNotificationDto>>("TribeNotifications");

            if (tribeNotificationsFromStorage is not null && tribeNotificationsFromStorage.Any())
            {
                var tribeNotificationsFromStorageMinusOutdated = tribeNotificationsFromStorage.Where(x => x.Added > DateTime.UtcNow.AddDays(-1));
                TribeNotifications.AddRange(tribeNotificationsFromStorageMinusOutdated);
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

        internal async Task Add(TribeNotificationDto dto)
        {
            var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<TribeNotificationDto>>("TribeNotifications");

            TribeNotifications = tribeNotificationsFromStorage.ToList() ?? new List<TribeNotificationDto>();
            TribeNotifications.Add(dto);

            await _localStorageService.SetItemAsync<IEnumerable<TribeNotificationDto>>(
                "TribeNotifications",
                TribeNotifications.OrderByDescending(x => x.Added));
        }
    }
}
