using GameModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

                //12.11.2023
                //oba wyjątki dzieja sie blisko śmierci plemienia - bo po kilku odswiezeniach dziala.. :
                //{"The database operation was expected to affect 1 row(s), but actually affected 0 row(s); data may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=527962 for information on understanding and handling optimistic concurrency exceptions."} ==> sprawdz => "The second error was trying to update a model that without first pulling it from the database:" https://stackoverflow.com/questions/53676084/entity-framework-core-database-operation-expected-to-affect-1-rows-but-actual
                //{"The connection does not support MultipleActiveResultSets."} => https://stackoverflow.com/questions/46163437/getting-the-connection-does-not-support-multipleactiveresultsets-in-a-foreach !!SECOND answer
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
