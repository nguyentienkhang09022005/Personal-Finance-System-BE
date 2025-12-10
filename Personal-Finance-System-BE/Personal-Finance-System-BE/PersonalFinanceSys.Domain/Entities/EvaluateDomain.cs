using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class EvaluateDomain
    {
        public Guid IdEvaluate { get; set; }

        public int? Star { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdPost { get; set; }

        public Guid IdUser { get; set; }

        public virtual User? IdUserNavigation { get; set; }

        public EvaluateDomain(int star)
        {
            SetStar(star);
        }

        public void SetStar(int star)
        {
            if (star < 1 || star > 5)
            {
                throw new ArgumentException("Số sao phải từ 1 - 5!");
            }
            Star = star;
        }
    }
}
