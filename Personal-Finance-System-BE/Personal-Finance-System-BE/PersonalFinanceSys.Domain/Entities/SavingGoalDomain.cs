using CloudinaryDotNet;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class SavingGoalDomain
    {
        public Guid IdSaving { get; set; }

        public string SavingName { get; set; } = null!;

        public decimal TargetAmount { get; set; }

        public DateOnly TargetDate { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdUser { get; set; }

        public virtual ICollection<SavingDetail> SavingDetails { get; set; } = new List<SavingDetail>();

        public SavingGoalDomain(decimal targetAmount, DateOnly targetDate)
        {
            SetTargetAmount(targetAmount);
            SetTargetDate(targetDate);
        }

        public SavingGoalDomain(DateOnly targetDate)
        {
            SetTargetDate(targetDate);
        }

        public void SetTargetAmount(decimal targetAmount)
        {
            if (targetAmount < 0)
                throw new ArgumentException("Số tiền mục tiêu không thể nhỏ hơn 0!");

            TargetAmount = targetAmount;
        }

        public void SetTargetDate(DateOnly targetDate)
        {
            if (targetDate < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Ngày mục tiêu không thể là ngày trong quá khứ!");

            TargetDate = targetDate;
        }
    }
}
