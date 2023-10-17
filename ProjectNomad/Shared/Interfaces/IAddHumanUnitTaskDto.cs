using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IAddHumanUnitTaskDto
    {
        public int TribeId { get; }
        public int HumanUnitId { get;}
        public ETaskType Type { get; }

        public int LocalizationStart_X { get; }
        public int LocalizationStart_Y { get;}
        public int LocalizationEnd_X { get; }
        public int LocalizationEnd_Y { get; }
    }
}
