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

        // GET api/taskOrder/{tribeId}
        [HttpGet("{tribeId}")]
        public async Task<ActionResult<IEnumerable<IHumanUnitTaskOrder>>> Index(int tribeId)
        {
            var taskOrders = await _gameModule.GetHumanUnitTaskOrders(tribeId);
            return Ok(taskOrders);
        }

        // POST api/taskOrder/
        [HttpPost]
        public async Task<ActionResult> AddTask(AddHumanUnitTaskOrder humanUnitTaskOrder)
        {
            await _gameModule.AddHumanUnitTaskOrder(humanUnitTaskOrder.TribeId, 
                humanUnitTaskOrder.Type, 
                humanUnitTaskOrder.MapTileX, 
                humanUnitTaskOrder.MapTileY);

            return Ok();
        }

        // DELETE api/taskOrder/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            await _gameModule.DeleteHumanUnitTaskOrder(id);
            return Ok();
        }
    }
}
