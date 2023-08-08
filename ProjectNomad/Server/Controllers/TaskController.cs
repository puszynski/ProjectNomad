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

        // POST api/task/add-task
        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask(IHumanUnitTaskDto humanUnitTask)
        {
            await _gameModule.AddTask(humanUnitTask);
            return Ok();
        }


    }
}
