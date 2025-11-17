namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class NotificationRequest
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public Guid IdUser { get; set; }

        public Guid? IdRelated { get; set; }

        public string? RelatedType { get; set; }
    }
}
