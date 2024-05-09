using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InterestingBlogWebApp.API.Controllers
{
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public IActionResult SendNotification([FromBody] string message)
        {
            _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
            return Ok(new { Message = "Notification sent successfully!" });
        }
    }
}
