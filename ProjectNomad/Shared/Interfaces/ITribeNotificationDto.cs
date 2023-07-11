using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface ITribeNotification
    {
        public int HumanUnitId { get; }
        public DateTime Added { get; }
        public EHumanNotificationType Type { get; }
    }
}
