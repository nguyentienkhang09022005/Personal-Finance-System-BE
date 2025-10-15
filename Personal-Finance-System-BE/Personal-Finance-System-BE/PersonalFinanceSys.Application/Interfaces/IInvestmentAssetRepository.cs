using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentAssetRepository
    {
        Task AddInvestmentAssetAsync(InvestmentAssetDomain investmentAssetDomain);

        Task DeleteInvestmentAssetAsync(Guid idAsset);

        Task<bool> CheckExistInvestmentAssetAsync(Guid idAsset);

        Task<InvestmentAssetDomain> GetInfInvestmentAssetAsync(Guid idAsset);
    }
}
