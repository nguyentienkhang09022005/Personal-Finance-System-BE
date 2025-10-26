namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class SavingGoalUpdateRequest
    {
        public string? SavingName { get; set; }

        public DateOnly TargetDate { get; set; }

        public string? Description { get; set; }

        public string? UrlImage { get; set; }
    }
}
