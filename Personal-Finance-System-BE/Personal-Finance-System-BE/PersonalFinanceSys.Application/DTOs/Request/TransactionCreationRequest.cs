namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class TransactionCreationRequest
    {
        public string TransactionName { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string TransactionCategory { get; set; }

        public DateTime? TransactionDate { get; set; }

        public Guid? IdUser { get; set; }
    }
}
