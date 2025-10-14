using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

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

        // Add InvestmentFund
        public async Task<InvestmentFundDomain> AddInvestmentAsync(InvestmentFundDomain invesmentFundDomain)
        {
            var investment = _mapper.Map<InvestmentFund>(invesmentFundDomain);
            investment.IdFund = Guid.NewGuid();
            await _context.InvestmentFunds.AddAsync(investment);
            await _context.SaveChangesAsync();
            return _mapper.Map<InvestmentFundDomain>(investment);
        }

        // Delete InvestmentFund
        public async Task DeleteInvestmentFundAsync(Guid idFund)
        {
            var Fund = await _context.InvestmentFunds.FindAsync(idFund)
                ?? throw new NotFoundException("Không tìm thấy quỹ cần xóa!");

            _context.InvestmentFunds.Remove(Fund);
            await _context.SaveChangesAsync();
        }

        // Check InvestmentFund Exist
        public async Task<InvestmentFund> ExistInvestmentFund(Guid idFund)
        {
            var fund = await _context.InvestmentFunds
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(i => i.IdFund == idFund);

            return fund ?? throw new NotFoundException("Không tìm thấy quỹ");
        }
        public async Task<bool> CheckExistInvestmentFund(Guid idFund)
        {
            return await _context.InvestmentFunds
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(i => i.IdFund == idFund);
        }

        // Get Inf InvestmentFund
        public async Task<InvestmentFundDomain> GetInfInvestmentFundAsync(Guid idFund)
        { 
            var investmentFund = await _context.InvestmentFunds
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IdFund == idFund);

            if (investmentFund == null)
                throw new NotFoundException("Không tìm thấy thông tin của quỹ!");
            return _mapper.Map<InvestmentFundDomain>(investmentFund);
        }

        // Get List InvestmentFund
        public async Task<List<InvestmentFundDomain?>> GetListInvesmentFundDomains(Guid idUser)
        {
            var investments = await _context.InvestmentFunds
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .Where(u => u.IdUser == idUser)
                .ToListAsync();
            return _mapper.Map<List<InvestmentFundDomain?>>(investments);
        }

        // Update InvestmentFund
        public async Task<InvestmentFundDomain> UpdateInvestmentFundAsync(InvestmentFundDomain investmentFundDomain, InvestmentFund investmentFund)
        {
            _mapper.Map(investmentFundDomain, investmentFund);
            await _context.SaveChangesAsync();
            return _mapper.Map<InvestmentFundDomain>(investmentFund);
        }
    }
}
