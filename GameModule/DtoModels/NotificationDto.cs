using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record NotificationDto(int HumanUnitId,
        string HumanName,
        DateTime Added, 
        ENotificationType Type, 
        string? CustomValue) : INotification;
}
