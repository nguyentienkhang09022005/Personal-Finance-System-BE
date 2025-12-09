namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class MessageResponse
    {
        public Guid IdUser { get; set; }

        public string? Name { get; set; }

        public string? UrlAvatar { get; set; }

        public List<MessageDetailResponse>? MessageDetailResponses { get; set; }
    }

    public class MessageDetailResponse
    {
        public Guid IdMessage { get; set; }

        public Guid IdSender { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? SendAt { get; set; }
    }
}
