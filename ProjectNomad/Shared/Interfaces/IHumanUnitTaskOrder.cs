using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnitTaskOrder
    {
        public int Id { get; }
        public int TribeId { get; }
        public DateTime Added { get; }
        public EHumanUnitTaskType Type { get; }
        public bool IsInProgress { get; }
        public int? X { get; }
        public int? Y { get; }
    }
}
