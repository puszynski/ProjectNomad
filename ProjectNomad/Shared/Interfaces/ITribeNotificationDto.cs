using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface ITribeNotification
    {
        public int HumanUnitId { get; set; }
        public DateTime Added { get; set; }
        public EHumanNotificationType Type { get; set; }
    }
}
