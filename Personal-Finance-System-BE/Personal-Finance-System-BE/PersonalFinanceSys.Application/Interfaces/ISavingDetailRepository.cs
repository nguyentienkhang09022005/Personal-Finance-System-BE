using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface ISavingDetailRepository
    {
        Task<SavingDetailDomain> AddSavingDetailAsync(SavingDetailDomain savingDetailDomain);

        Task DeleteSavingDetailAsync(Guid idSavingDetail);

        Task<List<SavingDetailDomain>> GetListSavingDetailsAsync(Guid idSavingGoal);

    }
}
