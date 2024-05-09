using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using InterestingBlogWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InterestingBlogWebApp.API.Controllers
{
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;

        public NotificationsController(IHubContext<NotificationHub> hubContext,
             INotificationService notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public IActionResult SendNotification([FromBody] string message)
        {
            _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
            return Ok(new { Message = "Notification sent successfully!" });
        }

        [HttpGet("count-unseen")]
        public async Task<IActionResult> CountUnseenNotifications()
        {
            var userId = User.FindFirst("userId")?.Value;
            var count = await _notificationService.CountUnreadNotifications(userId);
            return Ok(new { UnseenCount = count });
        }

        [HttpPost("mark-seen")]
        public async Task<IActionResult> MarkNotificationsAsSeen()
        {
            var userId = User.FindFirst("userId")?.Value;
            await _notificationService.MarkNotificationsAsRead(userId);
            return Ok(new { Message = "All notifications marked as seen." });
        }
    }
}
