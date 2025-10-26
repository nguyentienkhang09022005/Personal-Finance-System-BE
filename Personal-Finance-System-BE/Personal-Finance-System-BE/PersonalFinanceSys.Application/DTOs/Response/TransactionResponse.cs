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

    public class TransactionChartResponse
    {
        public string? TransactionCategory { get; set; }

        public decimal ExpensePercent { get; set; }

        public decimal ExpenseAmount { get; set; }
    }

    public class TransactionSummaryResponse
    {
        public List<TransactionResponse> ExpenseList { get; set; } = new();
        public List<TransactionChartResponse> ChartList { get; set; } = new();
    }
}
