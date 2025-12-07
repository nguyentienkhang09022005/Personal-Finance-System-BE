using Microsoft.AspNetCore.SignalR;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Hubs;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class MessageHubService : IMessageHubService
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageHubService(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PushMessageToUserAsync(Guid idUser, MessageResponse message)
        {
            await _hubContext.Clients.User(idUser.ToString())
                .SendAsync("ReceiveMessage", message);
        }
    }
}
