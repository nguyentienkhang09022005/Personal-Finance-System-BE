namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response
{
    public class CompareTransactionByMonthResponse
    {
        public MonthlyTransactionSummary Month1Summary { get; set; } = new();

        public MonthlyTransactionSummary Month2Summary { get; set; } = new();

        public decimal SpreadIncomeAndExpense { get; set; }
    }
    public class MonthlyTransactionSummary
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public decimal TotalIncome { get; set; }
        public List<CompareTransactionDetailResponse> TransactionIncomeDetails { get; set; } = new();

        public decimal TotalExpense { get; set; }
        public List<CompareTransactionDetailResponse> TransactionExpenseDetails { get; set; } = new();

        public decimal SpreadIncomeAndExpenseByYear { get; set; }
    }
}
