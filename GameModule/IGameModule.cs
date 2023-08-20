using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    public interface IGameModule
    {
        public Task InitPlayerGameObjects(Guid accountId);
        public Task InitPlayerGameObjectsForExistingTribe(Guid accountId);

        public Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId);

        // CONCEPT
        // server is not running tasks in background,
        // instead its triggered by client of main player - or - other players that are coming into interaction
        public Task TriggerPlayerGameObjectRecalculation(Guid accountId);

        public Task AddTask(IAddHumanUnitTaskDto task);
        public Task<IEnumerable<IHumanUnitTaskDto>> GetActualTribeTasks(int tribeId);

        // returns 7x7, x&y is pointing to the middle of teh square
        public Task<IEnumerable<IMapTile>> GetMapTiles(int tribeId);

        // NOTE
        // map tiles can be generated whole sections, eg. from 100|100 to 199|199
        public Task GenerateMapTiles(int x, int y);
    }
}
