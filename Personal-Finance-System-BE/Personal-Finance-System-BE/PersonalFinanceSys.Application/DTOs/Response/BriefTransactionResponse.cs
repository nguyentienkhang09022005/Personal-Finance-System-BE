namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class BriefTransactionResponse
    {
        public int totalTransactionInWeek { get; set; }

        public List<ListBriefTransactionResponse> listBriefTransactionResponses { get; set; }
    }

    public class ListBriefTransactionResponse
    {
        public Guid IdTransaction { get; set; }

        public string TransactionName { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
