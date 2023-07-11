using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace NotificationModule.Entities
{
    internal class TribeNotification
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public DateTime Updated { get; set; }
        public string Content { get; set; }
    }

    //not - only to convert List<ContentItem> to TribeNotification.Content
    internal class TribeNotificationContentItem : ITribeNotification
    {
        public int HumanUnitId { get; set; }
        public EHumanNotificationType Type { get; set; }
        public DateTime Added { get; set; }
    }

    //todo WorldNotifications
    //todo AccountNotifications
}
