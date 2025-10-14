using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class InvestmentDetailRepository : IInvestmentDetailRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public InvestmentDetailRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddInvestmentDetailAsync(InvestmentDetailDomain investmentDetailDomain)
        {
            var investmentDetail = _mapper.Map<InvestmentDetail>(investmentDetailDomain);
            investmentDetail.IdDetail = Guid.NewGuid();
            await _context.InvestmentDetails.AddAsync(investmentDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvestmentDetailAsync(Guid idDetail)
        {
            var investmentDetail = await _context.InvestmentDetails.FindAsync(idDetail)
                            ?? throw new NotFoundException("Không tìm thấy chi tiết cần xóa!");

            _context.InvestmentDetails.Remove(investmentDetail);
            await _context.SaveChangesAsync();
        }
    }
}
