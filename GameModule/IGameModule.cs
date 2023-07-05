using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    public interface IGameModule
    {
        public Task InitPlayerGameObject(Guid accountId);
        public Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId);

        // CONCEPT
        // server is not running tasks in background,
        // instead its triggered by client of main player - or - other players that are coming into interaction
        public Task TriggerPlayerGameObjectRecalculation(int tribeId);
    }
}
