using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnitTaskDto
    {
        public int TribeId { get; }
        public int HumanUnitId { get; }
        public EHumanUnitTaskType Type { get; }
        public DateTime From { get; }
        public DateTime? To { get; }
    }
}
