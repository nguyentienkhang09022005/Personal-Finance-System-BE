namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class FriendshipResponse
    {
        public Guid IdFriendship { get; set; }

        public string? Status { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public InfFriendshipResponse? infFriendshipResponse { get; set; }
    }

    public class InfFriendshipResponse
    {
        public InfFriendOfFriendshipResponse? Sender { get; set; }

        public InfFriendOfFriendshipResponse? Receiver { get; set; }
    }

    public class InfFriendOfFriendshipResponse
    {
        public Guid IdUser { get; set; }

        public string? Name { get; set; }

        public string? UrlAvatar { get; set; }
    }
}
