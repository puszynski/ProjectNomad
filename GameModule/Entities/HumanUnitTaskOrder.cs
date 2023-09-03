using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Properties;

namespace GameModule.Entities
{
    internal class HumanUnitTaskOrder : IId, ITribeId
    {
        public int Id { get; set; }
        public int TribeId { get; set; }
        public DateTime Added { get; set; }
        public EHumanUnitTaskType Type { get; set; }
        public bool IsInProgress { get; set; }
        public int? MapTileId { get; set; }
    }
}
