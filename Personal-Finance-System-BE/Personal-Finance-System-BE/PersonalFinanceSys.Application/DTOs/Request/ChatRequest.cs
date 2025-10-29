namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class ChatRequest
    {
        public Guid IdUser { get; set; }

        public string UserMessage { get; set; }
    }

    public class MessageHistoryItem
    {
        public string Role { get; set; }

        public string Message { get; set; }
    }
}
