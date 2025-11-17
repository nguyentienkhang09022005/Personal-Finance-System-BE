using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface INotificationHubService
    {
        Task PushNotificationToUserAsync(Guid idUser, NotificationResponse notification);
    }
}
