using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    public interface IGameModule
    {
        public Task InitPlayerGameObjects(Guid accountId);
        public Task InitPlayerGameObjectsForExistingTribe(int tribeId);

        public Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId);

        // CONCEPT
        // server is not running tasks in background,
        // instead its triggered by client of main player - or - other players that are coming into interaction
        public Task TriggerPlayerGameObjectRecalculation(int tribeId);


        public Task<IEnumerable<IMapTile>> GetMapData(int x, int y);
    }
}
