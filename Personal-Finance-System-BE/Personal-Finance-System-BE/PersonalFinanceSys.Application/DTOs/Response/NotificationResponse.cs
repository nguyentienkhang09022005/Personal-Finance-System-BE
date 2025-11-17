namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class NotificationResponse
    {
        public Guid IdNotification { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? NotificationType { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? NotificationDate { get; set; }
    }
}
