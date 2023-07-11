using Microsoft.AspNetCore.Mvc;
using NotificationModule;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationModule _notificationModule;
        public NotificationController(INotificationModule notificationModule) 
            => _notificationModule = notificationModule;

        [HttpGet("get/{tribeId}")]
        public async Task<ActionResult<IEnumerable<ITribeNotification>>> Get(int tribeId)
        {
            var result = await _notificationModule.GetAndRemoveTribeNotifications(tribeId); //todo check flow
            return Ok(result);
        }
    }
}
