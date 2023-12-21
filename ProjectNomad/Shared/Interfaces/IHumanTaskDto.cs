using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanTaskDto
    {
        public int Id { get; }
        public int TribeId { get; }
        public string HumanUnitName { get; }
        public int HumanUnitId { get; }
        public ETaskType Type { get; }
        public DateTime From { get; }
        public DateTime To { get; }

        public int? LocalizationX { get; }
        public int? LocalizationY { get; }
    }
}
