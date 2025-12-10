using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class EvaluateRepository : IEvaluateRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public EvaluateRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EvaluateDomain> AddEvaluateAsync(EvaluateDomain evaluateDomain)
        {
            var evaluate = _mapper.Map<Evaluate>(evaluateDomain);
            evaluate.IdEvaluate = Guid.NewGuid();

            _context.Evaluates.Add(evaluate);
            await _context.SaveChangesAsync();
            return _mapper.Map<EvaluateDomain>(evaluate);
        }

        public async Task DeleteEvaluteAsync(Guid idEvaluate)
        {
            var evaluate = await _context.Evaluates
                .FindAsync(idEvaluate) ?? throw new NotFoundException("Không tìm thấy đánh giá cần xóa!");

            _context.Evaluates.Remove(evaluate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistEvaluate(Guid idEvaluate)
        {
            return await _context.Evaluates
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(e => e.IdEvaluate == idEvaluate);
        }

        public async Task<Evaluate> GetExistEvaluate(Guid idEvaluate)
        {
            var evaluate = await _context.Evaluates
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(e => e.IdEvaluate == idEvaluate);

            return evaluate ?? throw new NotFoundException("Không tìm thấy đánh giá!");
        }

        public async Task<List<EvaluateDomain>> GetListEvaluateByPostIdAsync(Guid idPost)
        {
            var evaluates = _context.Evaluates
                .Where(e => e.IdPost == idPost)
                .Include(e => e.IdUserNavigation)
                .AsNoTracking()
                .ToList();
            return _mapper.Map<List<EvaluateDomain>>(evaluates);
        }

        public async Task<EvaluateDomain> UpdateEvaluateAsync(EvaluateDomain evaluateDomain, Evaluate evaluate)
        {
            _mapper.Map(evaluateDomain, evaluate);

            await _context.SaveChangesAsync();
            return _mapper.Map<EvaluateDomain>(evaluate);
        }
    }
}
