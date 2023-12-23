using GameModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.DTOs.ServerToWasm;
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
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("getTribeHumansWithJobs/{tribeId}")]
        public async Task<ActionResult<IEnumerable<HumanWithJobsDto>>> GetTribeHumansWithJobs(Guid tribeId)
        {
            var result = await _gameModule.GetTribeHumans(tribeId);
            return Ok(result);
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
