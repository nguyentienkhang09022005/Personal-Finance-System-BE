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

        public bool? IsFriend { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? SendAt { get; set; }
    }
}
