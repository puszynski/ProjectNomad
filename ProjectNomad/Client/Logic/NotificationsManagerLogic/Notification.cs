using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Logic.NotificationsManagerLogic
{
    internal record Notification(int HumanUnitId, 
        DateTime Added,
        ENotificationType Type,
        string? CustomValue) : INotification;
}
