using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentDetailRepository
    {
        Task AddInvestmentDetailAsync(InvestmentDetailDomain investmentDetailDomain);

        Task DeleteInvestmentDetailAsync(Guid idDetail);
        Task<List<InvestmentDetailDomain>> GetListInvestmentDetailAsync(Guid idAsset);
    }
}
