using ProjectNomad.Shared.Interfaces;

namespace GameModule
{
    public interface IGameModule
    {
        public Task InitPlayerGameObject(Guid accountId);
        public Task<ITribeGameObjects> GetPlayerGameObject(Guid accountId);

        // CONCEPT
        // server is not running tasks in background,
        // instead its triggered by main player - or - other players that are coming to interaction with one
        public Task TrigerPlayerGameObjectRecalculation(Guid accountId);
    }
}
