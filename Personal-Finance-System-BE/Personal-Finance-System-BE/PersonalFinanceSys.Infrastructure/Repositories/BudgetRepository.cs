using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public BudgetRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BudgetDomain> AddBudgetAsync(BudgetDomain budgetDomain)
        {
            var budget = _mapper.Map<Budget>(budgetDomain);
            budget.IdBudget = Guid.NewGuid();

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return _mapper.Map<BudgetDomain>(budget);
        }

        public async Task DeleteBudgetAsync(Guid idBudget)
        {
            var budget = await _context.Budgets.FindAsync(idBudget)
                                        ?? throw new NotFoundException("Không tìm ngân sách cần xóa!");

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }

        public async Task<Budget> GetExistBudget(Guid idBudget)
        {
            var budget = await _context.Budgets
                            .IgnoreAutoIncludes()
                            .FirstOrDefaultAsync(b => b.IdBudget == idBudget);

            return budget ?? throw new NotFoundException("Không tìm mục tiêu tiết kiệm!");
        }

        public async Task<bool> ExistBudget(Guid idBudget)
        {
            return await _context.Budgets
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(b => b.IdBudget == idBudget);
        }

        public async Task<List<BudgetDomain>> GetListBudgetByUserIdAsync(Guid idUser)
        {
            var budgets = _context.Budgets
                            .Where(s => s.IdUser == idUser)
                            .AsNoTracking()
                            .ToList();
            return _mapper.Map<List<BudgetDomain>>(budgets);
        }

        public async Task<BudgetDomain> UpdateBudgetAsync(BudgetDomain budgetDomain, Budget budget)
        {
            _mapper.Map(budgetDomain, budget);

            await _context.SaveChangesAsync();
            return _mapper.Map<BudgetDomain>(budget);
        }
    }
}
