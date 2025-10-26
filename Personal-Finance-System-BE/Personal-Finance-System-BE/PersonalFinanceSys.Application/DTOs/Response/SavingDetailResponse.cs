namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class SavingDetailResponse
    {
        public Guid IdDetail { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
