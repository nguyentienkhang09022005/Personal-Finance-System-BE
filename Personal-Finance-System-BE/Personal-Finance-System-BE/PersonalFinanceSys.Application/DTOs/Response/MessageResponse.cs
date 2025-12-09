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

        public string Content { get; set; } = null!;

        public DateTime? SendAt { get; set; }
    }

    public class ListMessageResponse
    {
        public Guid IdUser { get; set; }

        public string? NameUser { get; set; }

        public string? UrlAvatarUser { get; set; }

        public Guid IdRef { get; set; }

        public string? NameRef { get; set; }

        public string? UrlAvatarRef { get; set; }

        public List<MessageDetailResponse>? MessageDetailResponses { get; set; }
    }
}
