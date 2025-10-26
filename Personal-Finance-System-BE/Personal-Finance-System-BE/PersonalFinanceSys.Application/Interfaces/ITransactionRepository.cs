using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionDomain> AddTransactionAsync(TransactionDomain transactionDomain);

        Task DeleteTransactionAsync(Guid idTransaction);

        Task<TransactionDomain> UpdateTransactionAsync(TransactionDomain transactionDomain, Transaction transaction);

        Task<List<TransactionDomain>> GetListTransactionAsync(Guid idUser);

        Task<TransactionDomain> GetTransactionByIdAsync(Guid idTransaction);

        Task<Transaction> ExistTransaction(Guid idTransaction);

        Task UpdateTransactionCategoryByBudgetNameAsync(Guid idUser, string oldCategoryName, string newCategoryName);

        Task<List<TransactionDomain>> GetExpenseTransactionsByUserAsync(Guid idUser);
    }
}
