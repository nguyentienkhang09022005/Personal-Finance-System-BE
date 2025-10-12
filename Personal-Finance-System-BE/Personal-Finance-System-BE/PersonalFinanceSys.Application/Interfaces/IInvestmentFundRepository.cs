using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IInvestmentFundRepository
    {
        Task AddInvestmentAsync(InvesmentFundDomain invesmentFundDomain);

        Task<List<InvesmentFundDomain>>  GetListInvesmentFundDomains(Guid idUser);
    }
}
