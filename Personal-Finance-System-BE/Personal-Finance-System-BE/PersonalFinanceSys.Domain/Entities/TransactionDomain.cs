using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities
{
    public class TransactionDomain
    {
        public Guid IdTransaction { get; set; }

        public string TransactionName { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionCategory { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? IdUser { get; set; }

        public TransactionDomain(decimal amount, string transactionType) 
        {
            SetAmount(amount);
            SetType(transactionType);
        }

        public void SetAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Số tiền không được nhỏ hơn 0!");
            }
            Amount = amount;
        }

        public void SetType(string transactionType)
        {
            if (transactionType != ConstrantCollectAndExpense.TypeCollect && transactionType != ConstrantCollectAndExpense.TypeExpense)
            {
                throw new ArgumentException("Loại giao dịch không hợp lệ!");
            }
            TransactionType = transactionType;
        }
    }
}
