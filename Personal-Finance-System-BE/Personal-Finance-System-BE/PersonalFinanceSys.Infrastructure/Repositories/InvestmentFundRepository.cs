using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class InvestmentFundRepository : IInvestmentFundRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public InvestmentFundRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddInvestmentAsync(InvesmentFundDomain invesmentFundDomain)
        {
            var investment = _mapper.Map<InvestmentFund>(invesmentFundDomain);
            await _context.InvestmentFunds.AddAsync(investment);
            await _context.SaveChangesAsync();
        }

        public Task<List<InvesmentFundDomain>> GetListInvesmentFundDomains(Guid idUser)
        {
            throw new NotImplementedException();
        }
    }
}
