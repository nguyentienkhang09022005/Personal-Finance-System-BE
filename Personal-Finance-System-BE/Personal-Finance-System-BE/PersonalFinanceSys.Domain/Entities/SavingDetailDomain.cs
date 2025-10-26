namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class SavingDetailDomain
    {
        public Guid IdDetail { get; set; }

        public decimal Amount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? IdSaving { get; set; }

        public SavingDetailDomain(decimal amount)
        {
            SetAmount(amount);
        }

        public void SetAmount(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Số tiền phải lớn hơn 0!");

            Amount = amount;
        }
    }
}
