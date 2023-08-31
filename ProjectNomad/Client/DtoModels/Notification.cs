using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.DtoModels
{
    public record Notification(int HumanUnitId,
        string HumanUnitName,
        DateTime Added,
        ENotificationType Type,
        string? CustomValue) : INotification;
}
