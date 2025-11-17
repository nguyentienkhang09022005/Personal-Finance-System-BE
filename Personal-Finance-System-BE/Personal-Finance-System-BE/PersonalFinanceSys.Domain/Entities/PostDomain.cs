using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class PostDomain
    {
        public Guid IdPost { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public Guid IdUser { get; set; }

        public bool? IsApproved { get; set; }

        public string? Snapshot { get; set; }

        public virtual User? IdUserNavigation { get; set; }

        public PostDomain() { }
    }
}
