using GameModule;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Server.Models.RequestModels;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskOrderController : ControllerBase
    {
        readonly IGameModule _gameModule;
        public TaskOrderController(IGameModule gameModule) => _gameModule = gameModule;
                
        [HttpGet("{tribeId}")] // GET api/taskOrder/{tribeId}
        public async Task<ActionResult<IEnumerable<IHumanUnitTaskOrder>>> Index(int tribeId)
        {
            var taskOrders = await _gameModule.GetHumanUnitTaskOrders(tribeId);
            return Ok(taskOrders);
        }
                
        [HttpPost] // POST api/taskOrder/
        public async Task<ActionResult> AddTask(AddHumanUnitTaskOrder humanUnitTaskOrder)
        {
            //validation - only one order for: light a fire; only one from two: keep fire low OR keep fire big

            await _gameModule.AddHumanUnitTaskOrder(humanUnitTaskOrder.TribeId, 
                humanUnitTaskOrder.Type, 
                humanUnitTaskOrder.MapTileX, 
                humanUnitTaskOrder.MapTileY);

            return Ok();
        }
        
        [HttpDelete("{id}")] // DELETE api/taskOrder/{id}
        public async Task<ActionResult> DeleteTask(int id)
        {
            await _gameModule.DeleteHumanUnitTaskOrder(id);
            return Ok();
        }
    }
}
