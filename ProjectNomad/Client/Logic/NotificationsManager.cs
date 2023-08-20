using Blazored.LocalStorage;
using ProjectNomad.Client.Logic.NotificationsManagerLogic;

namespace ProjectNomad.Client.Logic
{
    internal class NotificationsManager
    {
        const string LOCAL_STORAGE_KEY = "Notifications";
        
        readonly ILocalStorageService _localStorageService;

        public NotificationsManager(ILocalStorageService localStorageService) 
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

        internal async Task Add(int humanUnitId, ENotificationType Type)
        {
            var notification = new Notification(humanUnitId, DateTime.UtcNow, Type);

            var tribeNotificationsFromStorage = await _localStorageService.GetItemAsync<IEnumerable<Notification>>(LOCAL_STORAGE_KEY) ?? new List<Notification>();

            var notificationsList = tribeNotificationsFromStorage.ToList();
            notificationsList.Add(notification);

            await _localStorageService.SetItemAsync(LOCAL_STORAGE_KEY, notificationsList);
        }
    }
}
