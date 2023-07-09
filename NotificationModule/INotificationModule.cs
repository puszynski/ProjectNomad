using ProjectNomad.Shared.Interfaces;

namespace NotificationModule
{
    public interface INotificationModule
    {
        public Task InsertTribeNotification(int tribeId, ITribeNotification notificationDTo);
        public Task<IEnumerable<ITribeNotification>> GetAndRemoveTribeNotifications(int tribeId);
    }
}
