namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class MessageDomain
    {
        public Guid IdMessage { get; set; }

        public string Content { get; set; } = null!;

        public bool? IsFriend { get; set; }

        public DateTime? SendAt { get; set; }

        public Guid? IdFriendship { get; set; }

        public Guid? IdUser { get; set; }

        public MessageDomain(){}
    }
}
