using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public PermissionRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PermissionDomain?>> GetListPermissionAsync()
        {
            var permissions = await _context.Permissions
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<PermissionDomain?>>(permissions);
        }
    }
}
