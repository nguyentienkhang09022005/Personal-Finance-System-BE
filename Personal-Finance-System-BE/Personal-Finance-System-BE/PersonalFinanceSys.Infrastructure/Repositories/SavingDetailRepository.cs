using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class SavingDetailRepository : ISavingDetailRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IDbContextFactory<PersonFinanceSysDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public SavingDetailRepository(PersonFinanceSysDbContext context, 
                                      IMapper mapper,
                                      IDbContextFactory<PersonFinanceSysDbContext> contextFactory)
        {
            _context = context;
            _mapper = mapper;
            _contextFactory = contextFactory;
        }
        public async Task<SavingDetailDomain> AddSavingDetailAsync(SavingDetailDomain savingDetailDomain)
        {
            var savingDetail = _mapper.Map<SavingDetail>(savingDetailDomain);
            savingDetail.IdDetail = Guid.NewGuid();

            _context.SavingDetails.Add(savingDetail);
            await _context.SaveChangesAsync();
            return _mapper.Map<SavingDetailDomain>(savingDetail);
        }

        public async Task DeleteSavingDetailAsync(Guid idSavingDetail)
        {
            var savingDetail = await _context.SavingDetails.FindAsync(idSavingDetail)
                                        ?? throw new NotFoundException("Không tìm lịch sử mục tiêu cần xóa!");

            _context.SavingDetails.Remove(savingDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SavingDetailDomain>> GetListSavingDetailsAsync(Guid idSavingGoal)
        {
            var savingDetails = _context.SavingDetails
                                        .Where(d => d.IdSaving == idSavingGoal)
                                        .IgnoreAutoIncludes()
                                        .AsNoTracking()
                                        .ToList();
            return _mapper.Map<List<SavingDetailDomain>>(savingDetails);
        }

        public async Task<List<SavingDetailDomain>> GetSavingDetailsByUserIdAsync(Guid idUser)
        {
            await using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<List<SavingDetailDomain>>(await context.SavingDetails
                .Include(d => d.IdSavingNavigation)
                .Where(d => d.IdSavingNavigation.IdUser == idUser)
                .AsNoTracking()
                .ToListAsync());
        }
    }
}
