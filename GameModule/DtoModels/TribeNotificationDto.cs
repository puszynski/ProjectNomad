using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record TribeNotificationDto(int HumanUnitId, DateTime Added, EHumanNotificationType Type) : ITribeNotification
    { }
}
