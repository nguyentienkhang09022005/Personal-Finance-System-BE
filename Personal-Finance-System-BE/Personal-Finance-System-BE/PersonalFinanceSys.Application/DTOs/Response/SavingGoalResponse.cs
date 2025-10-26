namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class SavingGoalResponse
    {
        public Guid IdSaving { get; set; }

        public string? SavingName { get; set; }

        public decimal? CurrentAmount { get; set; }

        public decimal? TargetAmount { get; set; }

        public DateOnly? TargetDate { get; set; }

        public decimal RemainingAmount { get; set; }

        public double SavingProgress { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public string? UrlImage { get; set; }

        public DateTime? CreateAt { get; set; }

        public List<SavingDetailResponse>? SavingDetail { get; set; }
    }
}
