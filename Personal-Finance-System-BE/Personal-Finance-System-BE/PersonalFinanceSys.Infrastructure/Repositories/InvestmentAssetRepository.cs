using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;
using System.Linq;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class InvestmentAssetRepository : IInvestmentAssetRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IDbContextFactory<PersonFinanceSysDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public InvestmentAssetRepository(PersonFinanceSysDbContext context, 
                                         IMapper mapper, 
                                         IDbContextFactory<PersonFinanceSysDbContext> contextFactory)
        {
            _context = context;
            _mapper = mapper;
            _contextFactory = contextFactory;
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

        public async Task<bool> CheckExistAssetAsync(string id)
        {
            return await _context.InvestmentAssets
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(i => i.Id == id);
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

        public async Task<List<InvestmentAssetDomain>> GetAssetsForMultipleFundsAsync(List<Guid> fundIds)
        {
            if (fundIds == null || !fundIds.Any())
            {
                return new List<InvestmentAssetDomain>();
            }

            var listInvestmentAssetEntity = await _context.InvestmentAssets
                .Where(asset => fundIds.Contains(asset.IdFund))
                .ToListAsync();

            var investmentAssetMap = _mapper.Map<List<InvestmentAssetDomain>>(listInvestmentAssetEntity);
            return investmentAssetMap;
        }

        public async Task<List<InvestmentAssetDomain>> GetAllAssetsByUserAsync(Guid idUser)
        {
            await using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<List<InvestmentAssetDomain>>(
                await context.InvestmentAssets
                .Include(a => a.IdFundNavigation)
                .Where(a => a.IdFundNavigation.IdUser == idUser)
                .AsNoTracking()
                .ToListAsync()
            );
        }
    }
}
