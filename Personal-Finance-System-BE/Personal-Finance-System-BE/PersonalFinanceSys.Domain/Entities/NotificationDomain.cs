using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class NotificationDomain
    {
        public Guid IdNotification { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? NotificationType { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? NotificationDate { get; set; }

        public Guid? IdUser { get; set; }

        public Guid? IdRelated { get; set; }

        public string? RelatedType { get; set; }

        public virtual User? IdUserNavigation { get; set; }

        public NotificationDomain() { }
    }
}
