using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface IHumanUnitTaskDto
    {
        public int TribeId { get; set; }
        public int HumanUnitId { get; set; }
        public EHumanUnitTaskType Type { get; set; }
        public DateTime From { get; set; }

        public int LocalizationStart_X { get; set; }
        public int LocalizationStart_Y { get; set; }
        public int LocalizationEnd_X { get; set; }
        public int LocalizationEnd_Y { get; set; }

        //todo all data needed to evaluate human unit skills..
        public int HumanUnit_FoodLevelPercentage { get; set; }

        public int MapTileId { get; set; }
    }
}
