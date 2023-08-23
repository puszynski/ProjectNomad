using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface INotification
    {
        int HumanUnitId { get; }
        DateTime Added { get; }
        ENotificationType Type { get; }
        string? CustomValue { get; }
    }
}
