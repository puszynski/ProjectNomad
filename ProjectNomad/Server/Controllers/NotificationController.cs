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


        // GET api/notification/get-notifications/{tribeId}
        [HttpGet("get-notifications/{tribeId}")]
        public async Task<ActionResult<IEnumerable<ITribeNotification>>> GetNotifications(int tribeId)
        {
            var result = await _notificationModule.GetAndRemoveTribeNotifications(tribeId);
            return Ok(result);
        }
    }
}
