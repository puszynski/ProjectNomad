using GameModule;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameModule _gameModule;
        public GameController(IGameModule gameModule)
        {
            _gameModule = gameModule;
        }

        [HttpGet("getGameObjects")]
        public async Task<ITribeGameObjects> GetGameObjects(Guid accountId) 
        {
            return await _gameModule.GetPlayerGameObject(accountId);
        }
    }
}
