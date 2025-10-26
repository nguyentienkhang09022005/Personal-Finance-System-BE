using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface ISavingDetailRepository
    {
        Task<SavingDetailDomain> AddSavingDetailAsync(SavingDetailDomain savingDetailDomain);

        Task DeleteSavingDetailAsync(Guid idSavingDetail);

        Task<List<SavingDetailDomain>> GetListSavingDetailsAsync(Guid idSavingGoal);

        Task<List<SavingDetailDomain>> GetSavingDetailsByUserIdAsync(Guid idUser);
    }
}
