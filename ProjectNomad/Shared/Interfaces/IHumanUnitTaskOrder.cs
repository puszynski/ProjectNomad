using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnitTaskOrder
    {
        public int Id { get; }
        public int TribeId { get; }
        public DateTime Added { get; }
        public ETaskType Type { get; }
        public int? HumanTaskId { get; }
        public int? X { get; }
        public int? Y { get; }
    }
}
