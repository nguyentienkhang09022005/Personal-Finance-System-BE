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
    public class SavingGoalRepository : ISavingGoalRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public SavingGoalRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SavingGoalDomain> AddSavingGoalAsync(SavingGoalDomain savingGoalDomain)
        {
            var savingGoal = _mapper.Map<SavingGoal>(savingGoalDomain);
            savingGoal.IdSaving = Guid.NewGuid();
            savingGoal.Status = ConstrantStatusSavingGoal.SavingGoalStatusPending;

            _context.SavingGoals.Add(savingGoal);
            await _context.SaveChangesAsync();
            return _mapper.Map<SavingGoalDomain>(savingGoal);
        }

        public async Task DeleteSavingGoalAsync(Guid idSavingGoal)
        {
            var savingGoal = await _context.SavingGoals.FindAsync(idSavingGoal)
                            ?? throw new NotFoundException("Không tìm thấy mục tiêu tiết kiệm cần xóa!");

            _context.SavingGoals.Remove(savingGoal);
            await _context.SaveChangesAsync();
        }

        public async Task<SavingGoal> GetExistSavingGoal(Guid idSavingGoal)
        {
            var savingGoal = await _context.SavingGoals
                            .IgnoreAutoIncludes()
                            .FirstOrDefaultAsync(t => t.IdSaving == idSavingGoal);

            return savingGoal ?? throw new NotFoundException("Không tìm thấy mục tiêu tiết kiệm!");
        }

        public async Task<bool> ExistSavingGoalDomain(Guid idSavingGoal)
        {
            return await _context.SavingGoals
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(u => u.IdSaving == idSavingGoal);
        }


        public async Task<List<SavingGoalDomain>> GetListSavingGoalsAsync(Guid idUser)
        {
            var savingGoals = _context.SavingGoals
                            .Where(s => s.IdUser == idUser)
                            .Include(s => s.SavingDetails)
                            .AsNoTracking()
                            .ToList();
            return _mapper.Map<List<SavingGoalDomain>>(savingGoals);
        }

        public async Task<SavingGoalDomain> GetSavingGoalByIdAsync(Guid idSavingGoal)
        {
            var savingGoal = _context.SavingGoals
                            .Include(s => s.SavingDetails)
                            .AsNoTracking()
                            .FirstOrDefault(s => s.IdSaving == idSavingGoal);

            if (savingGoal == null)
                throw new NotFoundException("Không tìm thấy thông tin của mục tiêu tiết kiệm!");
            return _mapper.Map<SavingGoalDomain>(savingGoal);
        }

        public async Task<SavingGoalDomain> UpdateSavingGoalAsync(SavingGoalDomain savingGoalDomain, SavingGoal savingGoal)
        {
            _mapper.Map(savingGoalDomain, savingGoal);

            await _context.SaveChangesAsync();
            return _mapper.Map<SavingGoalDomain>(savingGoal);
        }
    }
}
