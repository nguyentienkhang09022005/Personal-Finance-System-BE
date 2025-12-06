using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<NotificationDomain> AddNotificationAsync(NotificationDomain notificationDomain);

        Task<List<NotificationDomain?>> GetListNotificationByUserIdAsync(Guid idUser);

        Task DeleteNotificationAsync(Guid idNotification);

        Task<Notification> GetExistNotificationAsync(Guid idNotification);

        Task UpdateNotificationAsync(NotificationDomain notificationDomain, Notification notification);
    }
}
