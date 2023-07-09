using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace NotificationModule.DtoModels
{
    internal class TribeNotificationDto : ITribeNotification
    {
        public int HumanUnitId { get; set; }
        public DateTime Added { get; set; }
        public EHumanNotificationType Type { get; set; }
    }
}
