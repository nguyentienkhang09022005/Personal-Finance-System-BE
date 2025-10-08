using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class InvalidatedTokenRepository : IInvalidatedTokenRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public InvalidatedTokenRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> ExistByIdAsync(Guid idToken)
        {
            var exists = _context.InvalidatedTokens.Any(t => t.IdToken == idToken);
            return Task.FromResult(exists);
        }

        public async Task AddInvalidatedTokenAsync(InvalidatedTokenDomain invalidatedTokenDomain)
        {
            var invalidatedToken = _mapper.Map<InvalidatedToken>(invalidatedTokenDomain);
            await _context.InvalidatedTokens.AddAsync(invalidatedToken);
            await _context.SaveChangesAsync();
        }
    }
}
