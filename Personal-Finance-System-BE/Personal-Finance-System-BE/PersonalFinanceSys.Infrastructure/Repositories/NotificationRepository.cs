using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NotificationDomain> AddNotificationAsync(NotificationDomain notificationDomain)
        {
            var notification = _mapper.Map<Notification>(notificationDomain);
            notification.IdNotification = Guid.NewGuid();
            notification.IsRead = false;

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return _mapper.Map<NotificationDomain>(notification);
        }

        public async Task DeleteNotificationAsync(Guid idNotification)
        {
            var notification = await _context.Notifications.FindAsync(idNotification)
                                        ?? throw new NotFoundException("Không tìm thông báo cần xóa!");

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<Notification> GetExistPostAsync(Guid idNotification)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == idNotification);

            return notification ?? throw new NotFoundException("Không tìm thấy thông báo!");
        }

        public async Task<List<NotificationDomain?>> GetListNotificationByUserIdAsync(Guid idUser)
        {
            var notifications = _context.Notifications
                .Where(n => n.IdUser == idUser)
                .AsNoTracking()
                .ToList();
            return _mapper.Map<List<NotificationDomain>>(notifications);
        }
    }
}
