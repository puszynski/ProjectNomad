using GameModule.Entities;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace GameModule
{
    public interface IGameModule
    {
        public Task InitPlayerGameObjects(Guid accountId);
        public Task InitPlayerGameObjectsForExistingTribe(Guid accountId);

        // CONCEPT
        // server is not running tasks in background,
        // instead its triggered by client of main player - or - other players that are coming into interaction
        public Task<ITriggerGameLooperResponse> TriggerPlayerGameObjectRecalculation(Guid accountId);

        public Task AddTask(IAddHumanUnitTaskDto task);
        public Task<IEnumerable<IHumanUnitTaskDto>> GetActualTribeTasks(int tribeId);

        // returns 7x7, x&y is pointing to the middle of teh square
        public Task<IEnumerable<IMapTile>> GetMapTiles(int tribeId);

        //get larger count of tiles (depends on mini-map size)
        public Task<IEnumerable<IMapTile>> GetMapTileForMiniMap(int tribeId, int miniMapSizeInTiles = 31);

        // NOTE
        // map tiles can be generated whole sections, eg. from 100|100 to 199|199
        public Task GenerateMapTiles(int x, int y);


        public Task<IEnumerable<IHumanUnitTaskOrder>> GetHumanUnitTaskOrders(int tribeId);
        public Task AddHumanUnitTaskOrder(int tribeId, ETaskType type, int mapTileX, int mapTileY);
        public Task DeleteHumanUnitTaskOrder(int id);
    }
}
