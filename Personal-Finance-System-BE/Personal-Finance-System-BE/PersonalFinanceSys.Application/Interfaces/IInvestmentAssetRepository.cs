using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentAssetRepository
    {
        Task<InvestmentAssetDomain> AddInvestmentAssetAsync(InvestmentAssetDomain investmentAssetDomain);

        Task DeleteInvestmentAssetAsync(Guid idAsset);

        Task<bool> CheckExistInvestmentAssetAsync(Guid idAsset);

        Task<InvestmentAssetDomain> GetInfInvestmentAssetAsync(Guid idAsset);

        Task<List<InvestmentAssetDomain>> GetListInvestmentAssetAsync(Guid idFund);

        Task<List<InvestmentAssetDomain>> GetListInvestmentAssetByUserAsync(Guid idUser);

        Task<List<InvestmentAssetDomain>> GetAssetsForMultipleFundsAsync(List<Guid> fundIds);

        Task<List<InvestmentAssetDomain>> GetAllAssetsByUserAsync(Guid idUser);

        Task<bool> CheckExistInvestmentAssetByIdAsync(string id);
    }
}
