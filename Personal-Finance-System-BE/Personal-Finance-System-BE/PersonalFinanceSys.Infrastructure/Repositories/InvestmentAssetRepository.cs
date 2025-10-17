using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class InvestmentAssetRepository : IInvestmentAssetRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public InvestmentAssetRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvestmentAssetDomain> AddInvestmentAssetAsync(InvestmentAssetDomain investmentAssetDomain)
        {
            var investmentAsset = _mapper.Map<InvestmentAsset>(investmentAssetDomain);
            investmentAsset.IdAsset = Guid.NewGuid();
            await _context.InvestmentAssets.AddAsync(investmentAsset);
            await _context.SaveChangesAsync();
            return _mapper.Map<InvestmentAssetDomain>(investmentAsset);
        }

        public async Task DeleteInvestmentAssetAsync(Guid idAsset)
        {
            var investmentAsset = await _context.InvestmentAssets.FindAsync(idAsset)
                            ?? throw new NotFoundException("Không tìm thấy tài sản cần xóa!");

            _context.InvestmentAssets.Remove(investmentAsset);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckExistInvestmentAssetAsync(Guid idAsset)
        {
            return await _context.InvestmentAssets
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(i => i.IdAsset == idAsset);
        }

        public async Task<InvestmentAssetDomain> GetInfInvestmentAssetAsync(Guid idAsset)
        {
            var investmentAsset = await _context.InvestmentAssets
                    .AsNoTracking()
                    .IgnoreAutoIncludes()
                    .FirstOrDefaultAsync(i => i.IdAsset == idAsset);

            if (investmentAsset == null)
                throw new NotFoundException("Không tìm thấy thông tin tài sản!");
            return _mapper.Map<InvestmentAssetDomain>(investmentAsset);
        }

        public async Task<List<InvestmentAssetDomain>> GetListInvestmentAssetAsync(Guid idFund)
        {
            var listInvestmentAsset = await _context.InvestmentAssets
                .Where(i => i.IdFund == idFund)
                .AsNoTracking()
                .ToListAsync();

            if (listInvestmentAsset == null)
                throw new NotFoundException("Không tìm thấy danh sách tài sản!");
            return _mapper.Map<List<InvestmentAssetDomain>>(listInvestmentAsset);
        }
    }
}
