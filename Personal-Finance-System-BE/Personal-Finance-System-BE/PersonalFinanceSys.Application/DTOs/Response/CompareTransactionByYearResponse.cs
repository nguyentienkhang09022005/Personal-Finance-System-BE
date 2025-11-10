namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CompareTransactionByYearResponse
    {
        public YearlyTransactionSummary Year1Summary { get; set; } = new();

        public YearlyTransactionSummary Year2Summary { get; set; } = new();

        public decimal SpreadIncomeAndExpense { get; set; }
    }

    public class YearlyTransactionSummary
    {
        public int Year { get; set; }

        public decimal TotalIncome { get; set; }
        public List<CompareTransactionDetailResponse> TransactionIncomeDetails { get; set; } = new();

        public decimal TotalExpense { get; set; }
        public List<CompareTransactionDetailResponse> TransactionExpenseDetails { get; set; } = new();

        public decimal SpreadIncomeAndExpenseByYear { get; set; }
    }

    public class CompareTransactionDetailResponse
    {
        public Guid IdTransaction { get; set; }

        public string TransactionName { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionCategory { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
