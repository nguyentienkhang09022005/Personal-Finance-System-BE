using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentAssetRepository
    {
        Task<InvestmentAssetDomain> AddInvestmentAssetAsync(InvestmentAssetDomain investmentAssetDomain);

        Task DeleteInvestmentAssetAsync(Guid idAsset);

        Task<bool> CheckExistInvestmentAssetAsync(Guid idAsset);

        Task<bool> CheckExistAssetAsync(string id);

        Task<InvestmentAssetDomain> GetInfInvestmentAssetAsync(Guid idAsset);

        Task<List<InvestmentAssetDomain>> GetListInvestmentAssetAsync(Guid idFund);

        Task<List<InvestmentAssetDomain>> GetAssetsForMultipleFundsAsync(List<Guid> fundIds);
    }
}
