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
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;  //{"Invalid object name 'GameModule.TribeRelocations'."}


                //todo problem with new born or natural death?
                // NOT REOLVED {"The connection does not support MultipleActiveResultSets."} => https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/enabling-multiple-active-result-sets
            }
        }

        [HttpGet("getMapTiles/{tribeId}")]
        public async Task<ActionResult<IEnumerable<IMapTile>>> GetMapTiles(int tribeId)
        {
            var result = await _gameModule.GetMapTiles(tribeId);
            return Ok(result);
        }
    }
}
