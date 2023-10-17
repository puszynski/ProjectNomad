using GameModule;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Response;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameModule _gameModule;
        public GameController(IGameModule gameModule) 
            => _gameModule = gameModule;

        [HttpPost("triggerPlayerGameObjectRecalculation")]
        public async Task<ActionResult<ITriggerGameLooperResponse>> TriggerPlayerGameObjectRecalculation([FromBody]Guid accountId)
        {
            try
            {
                var result = await _gameModule.TriggerPlayerGameObjectRecalculation(accountId);

                if (result == null)
                    return BadRequest("result object from TriggerPlayerGameObjectRecalculation() is null");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // current problem:
                //{"The association between entity types 'Tribe' and 'HumanTask' has been severed, but the relationship is either marked as required or is implicitly required because the foreign key is not nullable. If the dependent/child entity should be deleted when a required relationship is severed, configure the relationship to use cascade deletes. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the key values."}

                //todo log
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getMapTiles/{tribeId}")]
        public async Task<ActionResult<IEnumerable<IMapTile>>> GetMapTiles(int tribeId)
        {
            var result = await _gameModule.GetMapTiles(tribeId);
            return Ok(result);
        }

        [HttpGet("getMiniMapTiles/{tribeId}")]
        public async Task<ActionResult<IEnumerable<IMapTile>>> GetMapTileForMiniMap(int tribeId)
        {
            var result = await _gameModule.GetMapTileForMiniMap(tribeId);
            return Ok(result);
        }
    }
}
