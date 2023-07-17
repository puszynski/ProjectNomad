using GameModule.Entities;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/SZ~y?qccOD!cf0a")]
    public class AdminController : ControllerBase
    {
#if DEBUG

        [HttpPost("GenerateNewMapSection")]
        public async Task<ActionResult> GenerateNewMapSection(int x_start, int y_start)
        {
            //todo valodate if x y rounded to 100

            var tiles = new List<IMapTile>();

            for (int x = x_start; x < x_start + 100; x++)
            {
                for (int y = y_start; y < y_start + 100; y++)
                {
                    var tile = new MapTile() { } //todo
                    tiles.Add()
                }
            }

            return Ok();
        }

#endif
    }
}
