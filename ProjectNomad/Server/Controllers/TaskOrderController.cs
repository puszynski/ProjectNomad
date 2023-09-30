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

        //todo - ideas - move to trello

        //OGIEŃ/ZIMNO
        //- order task na rozpalenie ognia - jedna osoba próbuje rozpalic - próba trwa krótko ale ma tylko jakieś prawdopodobieństwo że się uda,
        //	jak się nie uda, task order zostaje i znów ktoś próbuje, im gorsza pogoda tym mniesza szansa
        //- order task na utrzymanie małego/dużego ognia - mały daje mniej ciepła ale zużywa mnie drewna, ale moze sie zgasić w burzy, duży
        // więcej ciepla wiecej drewna ale nie gaśnie
        //- jakiś lvl ciepła na humanUnits..

        //Dzień/Noc + Sen
        //- w nocy śpią - automat
        //- w dzień śpią jak są krytycznie niewyspani lub głodni, chorzy, umierający.. - automat


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
