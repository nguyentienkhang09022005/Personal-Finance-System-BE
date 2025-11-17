using Microsoft.AspNetCore.SignalR;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Hubs;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PushNotificationToUserAsync(Guid idUser, NotificationResponse notification)
        {
            await _hubContext.Clients.User(idUser.ToString())
                .SendAsync("ReceiveNotification", notification);
        }
    }
}
