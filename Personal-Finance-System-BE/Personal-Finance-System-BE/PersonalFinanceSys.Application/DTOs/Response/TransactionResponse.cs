namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class TransactionResponse
    {
        public Guid IdTransaction { get; set; }

        public string TransactionName { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionCategory { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}
