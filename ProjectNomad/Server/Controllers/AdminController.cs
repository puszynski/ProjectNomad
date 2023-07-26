using GameModule;
using Microsoft.AspNetCore.Mvc;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        readonly IGameModule _gameModule;

        public AdminController(IGameModule gameModule)
        {
            _gameModule = gameModule;
        }

#if DEBUG

        //https://localhost:7277/api/admin/generatenewmapsection?x=0&y=0
        [HttpGet("GenerateNewMapSection")]
        public async Task<ActionResult> GenerateNewMapSection(int? x, int? y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            await _gameModule.GenerateMapTiles(x.Value, y.Value); //nie zapisuje sie, moze await i zrob akcje async?
            return Ok();
        }

#endif

    }
}
