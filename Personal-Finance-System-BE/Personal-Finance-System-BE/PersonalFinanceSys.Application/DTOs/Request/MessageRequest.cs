namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class MessageRequest
    {
        public Guid IdFriendship { get; set; }

        public Guid? IdUser { get; set; }

        public string? Content { get; set; }
    }
}
