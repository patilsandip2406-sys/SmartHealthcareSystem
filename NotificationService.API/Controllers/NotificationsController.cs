using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Storage;

namespace NotificationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetNotifications()
        {
            return Ok(NotificationStore.Notifications);
        }

        [HttpPost("{id}/mark-read")]
        public IActionResult MarkAsRead(int id)
        {
            var notification = NotificationStore.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification == null) return NotFound();

            notification.Status = "Read";
            return Ok(notification);
        }
    }
}
