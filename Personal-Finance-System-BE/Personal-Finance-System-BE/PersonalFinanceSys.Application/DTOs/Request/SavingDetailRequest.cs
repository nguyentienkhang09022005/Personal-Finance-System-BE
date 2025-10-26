namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class SavingDetailRequest
    {
        public decimal Amount { get; set; }

        public Guid? IdSaving { get; set; }
    }
}
