using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentDetailRepository
    {
        Task AddInvestmentDetailAsync(InvestmentDetailDomain investmentDetailDomain);

        Task DeleteInvestmentDetailAsync(Guid idDetail);

        Task<List<InvestmentDetailDomain>> GetListInvestmentDetailAsync(Guid idAsset);

        Task<decimal> GetNetQuantityForAssetAsync(Guid idAsset);

        Task<List<InvestmentDetailDomain>> GetAllDetailsByUserAsync(Guid idUser);

        Task<List<InvestmentDetailDomain>> GetInvestmentDetailsByUserAndMonthsAsync(Guid idUser, (int month, int year)[] periods);

        Task<List<InvestmentDetailDomain>> GetInvestmentDetailsByUserAndYearsAsync(Guid idUser, int[] years);
    }
}
