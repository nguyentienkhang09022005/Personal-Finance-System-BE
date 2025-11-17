namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request
{
    public class TransactionPostUpdateRequest
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? UrlImage { get; set; }

        public List<TransactionOfPost>? TransactionOfPost { get; set; }
    }
}
