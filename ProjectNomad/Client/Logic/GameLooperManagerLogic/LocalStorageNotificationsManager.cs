using Blazored.LocalStorage;
using ProjectNomad.Client.Models.Response;
using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Client.Logic.GameLooperManagerLogic
{
    internal class LocalStorageNotificationsManager
    {
        const string LOCAL_STORAGE_KEY = "Notifications";

        readonly ILocalStorageService _localStorageService;

        public LocalStorageNotificationsManager(ILocalStorageService localStorageService)
            => _localStorageService = localStorageService;

        internal async Task<IEnumerable<Notification>> Get()
        {
            var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<Notification>>(LOCAL_STORAGE_KEY) ?? new List<Notification>();

            if (!tribeNotificationsFromStorage.Any())
                return tribeNotificationsFromStorage;

            var actualNotifications = tribeNotificationsFromStorage
                .Where(x => x.Added > DateTime.UtcNow.AddDays(-1))
                .OrderByDescending(x => x.Added);

            await _localStorageService.SetItemAsync(LOCAL_STORAGE_KEY, actualNotifications);
            return actualNotifications;
        }

        internal async Task Clean() 
            => await _localStorageService.SetItemAsync(LOCAL_STORAGE_KEY, new List<Notification>());

        internal async Task Add(
            int humanUnitId,
            string humanUnitName,
            ENotificationType type,
            string? customValue)
        {
            var notification = new Notification(humanUnitId,
                humanUnitName,
                DateTime.UtcNow, 
                type,
                customValue);

            var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<Notification>>(LOCAL_STORAGE_KEY)
                ?? new List<Notification>();

            var notificationsList = tribeNotificationsFromStorage.ToList();
            notificationsList.Add(notification);

            await _localStorageService.SetItemAsync(LOCAL_STORAGE_KEY, notificationsList.OrderByDescending(x => x.Added));
        }
    }
}
