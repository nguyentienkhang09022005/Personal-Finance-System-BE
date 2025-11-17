using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IBudgetRepository
    {
        Task<BudgetDomain> AddBudgetAsync(BudgetDomain budgetDomain);

        Task DeleteBudgetAsync(Guid idBudget);

        Task<BudgetDomain> UpdateBudgetAsync(BudgetDomain budgetDomain, Budget budget);

        Task<List<BudgetDomain>> GetListBudgetByUserIdAsync(Guid idUser);

        Task<bool> ExistBudget(Guid idBudget);

        Task<Budget> GetExistBudget(Guid idBudget);
    }
}
