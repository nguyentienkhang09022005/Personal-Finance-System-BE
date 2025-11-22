using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class FriendshipDomain
    {
        public Guid IdFriendship { get; set; }

        public string? Status { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public Guid IdUser { get; set; }

        public Guid IdRef { get; set; }

        public virtual User? IdRefNavigation { get; set; }

        public virtual User? IdUserNavigation { get; set; }

        public FriendshipDomain() { }
    }
}
