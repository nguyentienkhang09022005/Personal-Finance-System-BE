namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class TransactionPostCreationRequest
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? UrlImage { get; set; }

        public Guid IdUser { get; set; }

        public List<TransactionOfPost>? TransactionOfPost { get; set; }
    }

    public class TransactionOfPost
    {
        public string Category { get; set; }

        public string TransactionName { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
