using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface ISavingGoalRepository
    {
        Task<SavingGoalDomain> AddSavingGoalAsync(SavingGoalDomain savingGoalDomain);

        Task DeleteSavingGoalAsync(Guid idSavingGoal);

        Task<SavingGoalDomain> UpdateSavingGoalAsync(SavingGoalDomain savingGoalDomain, SavingGoal savingGoal);

        Task<List<SavingGoalDomain>> GetListSavingGoalsAsync(Guid idUser);

        Task<SavingGoalDomain> GetSavingGoalByIdAsync(Guid idSavingGoal);

        Task<SavingGoal> GetExistSavingGoal(Guid idSavingGoal);

        Task<bool> ExistSavingGoalDomain(Guid idSavingGoal);
    }
}
