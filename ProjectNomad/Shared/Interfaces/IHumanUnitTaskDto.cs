using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnitTaskDto
    {
        public int Id { get; }
        public int TribeId { get; }
        public string HumanUnitName { get; }
        public int HumanUnitId { get; }
        public ETaskType Type { get; }
        public DateTime From { get; }
        public DateTime To { get; }
        bool IsCompleted { get; }
    }
}
