using GameModule;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        readonly IGameModule _gameModule;
        public TaskController(IGameModule gameModule)
        {
            _gameModule = gameModule;
        }

        // GET api/task/get-actual-tasks/{tribeId}
        [HttpGet("get-actual-tasks/{tribeId}")]
        public async Task<ActionResult<IEnumerable<IHumanUnitTaskDto>>> GetActualTasks(int tribeId)
        {
            var model = await _gameModule.GetActualTribeTasks(tribeId);
            return Ok(model);
        }

        // POST api/task/add-task
        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask(IAddHumanUnitTaskDto humanUnitTask)
        {
            await _gameModule.AddTask(humanUnitTask);
            return Ok();
        }


    }
}
