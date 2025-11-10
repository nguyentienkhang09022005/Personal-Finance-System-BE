namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class SavingGoalCreationRequest
    {
        public string SavingName { get; set; } = null!;

        public decimal TargetAmount { get; set; }

        public DateOnly TargetDate { get; set; }

        public string? Description { get; set; }

        public string? UrlImage { get; set; }

        public Guid IdUser { get; set; }
    }
}
