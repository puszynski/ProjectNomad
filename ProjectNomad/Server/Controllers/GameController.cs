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

        [HttpGet("getGameObjects/{accountId}")]
        public async Task<ActionResult<ITribeGameObjects>> GetGameObjects(Guid accountId) 
        {
            var model = await _gameModule.GetPlayerGameObject(accountId);
            return Ok(model);
        }
    }
}
