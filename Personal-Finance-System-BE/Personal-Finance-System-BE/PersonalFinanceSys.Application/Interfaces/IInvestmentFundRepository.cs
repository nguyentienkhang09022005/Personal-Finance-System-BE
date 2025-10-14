using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentFundRepository
    {
        Task<InvestmentFundDomain> AddInvestmentAsync(InvestmentFundDomain invesmentFundDomain);

        Task<List<InvestmentFundDomain>> GetListInvesmentFundDomains(Guid idUser);

        Task<InvestmentFund> ExistInvestmentFund(Guid idFund);

        Task<bool> CheckExistInvestmentFund(Guid idFund);

        Task<InvestmentFundDomain> GetInfInvestmentFundAsync(Guid idFund);

        Task DeleteInvestmentFundAsync(Guid idFund);

        Task<InvestmentFundDomain> UpdateInvestmentFundAsync(InvestmentFundDomain invesmentFund, InvestmentFund investmentFund);
    }
}
