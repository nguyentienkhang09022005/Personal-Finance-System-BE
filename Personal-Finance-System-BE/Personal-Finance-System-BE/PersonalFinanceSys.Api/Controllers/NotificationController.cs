using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Notifications;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Api.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationHandler _notificationHandler;

        public NotificationController(NotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        [Authorize]
        [HttpDelete("delete-notification")]
        public async Task<IActionResult> DeleteNotification([FromQuery] Guid idNotification)
        {
            var result = await _notificationHandler.DeleteNotificationAsync(idNotification);
            if (result.Success)
            {
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("update-is-read-notification")]
        public async Task<IActionResult> UpdateNotificationByUser([FromQuery] Guid idNotification)
        {
            var result = await _notificationHandler.UpdateNotificationAsync(idNotification);
            if (result.Success)
            {
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("list-notification-by-user")]
        public async Task<IActionResult> ListNotificationByUser([FromQuery] Guid idUser)
        {
            var result = await _notificationHandler.GetListNotificationByUserAsync(idUser);
            if (result.Success)
            {
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
