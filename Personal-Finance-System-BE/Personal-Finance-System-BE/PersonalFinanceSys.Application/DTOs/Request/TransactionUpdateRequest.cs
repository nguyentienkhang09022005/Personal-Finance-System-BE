namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class TransactionUpdateRequest
    {
        public string TransactionName { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionCategory { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
