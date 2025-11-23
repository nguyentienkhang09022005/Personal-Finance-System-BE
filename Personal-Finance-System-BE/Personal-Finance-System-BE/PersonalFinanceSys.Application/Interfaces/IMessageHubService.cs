using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IMessageHubService
    {
        Task PushMessageToUserAsync(Guid idUser, MessageDetailResponse messageDetailResponse);
    }
}
