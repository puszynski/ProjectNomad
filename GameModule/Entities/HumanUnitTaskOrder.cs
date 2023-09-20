using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces.Properties;

namespace GameModule.Entities
{
    //todo name "TaskOrder" or "TribeTaskOrder"
    internal class HumanUnitTaskOrder : IId, ITribeId
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }

        public DateTime Added { get; set; }
        public EHumanUnitTaskType Type { get; set; } 
        public bool IsInProgress { get; set; }

        //public int? MapTileId { get; set; }
        public Localization Localization { get; set; }



        //TODO CHANGE??  public Localization? DestinyMapTile
        //MapTileId                                     MapTile-X-Y
        //-must be fetch from db in server              +always avaliable data
        //-                                             +in WASM can be easily selected, event by hand via player
        //+less columns                                 -more
        //+download tiles by ID in gameLooper           --
    }
}
