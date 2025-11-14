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
    public class PackageRepository : IPackageRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public PackageRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PackageDomain> AddPackageAsync(PackageDomain packageDomain, List<string> permissionNames)
        {
            var package = _mapper.Map<Package>(packageDomain);
            package.IdPackage = Guid.NewGuid();

            var permissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.PermissionName))
                .ToListAsync();

            package.PermissionNames = permissions;

            await _context.Packages.AddAsync(package);
            await _context.SaveChangesAsync();
            return _mapper.Map<PackageDomain>(package);
        }

        public async Task<bool> CheckExistPackage(Guid idPackage)
        {
            return await _context.Packages
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(p => p.IdPackage == idPackage);
        }

        public async Task DeletePackageAsync(Guid idPackage)
        {
            var package = await _context.Packages.FindAsync(idPackage)
                            ?? throw new NotFoundException("Không tìm thấy gói cần xóa!");

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
        }

        public async Task<Package> ExistPackage(Guid idPackage)
        {
            var package = await _context.Packages
                            .IgnoreAutoIncludes()
                            .FirstOrDefaultAsync(p => p.IdPackage == idPackage);

            return package ?? throw new NotFoundException("Không tìm thấy gói");      
        }

        public async Task<(List<Package> AllPackages, List<Guid> BoughtPackageIds)>GetPackagesWithBoughtInfo(Guid idUser)
        {
            // Lấy danh sách các IdPackage mà user đã mua
            var boughtIds = await _context.Payments
                .Where(p => p.IdUser == idUser && p.Status == ConstantStatusPayment.PaymentSuccess)
                .Select(p => p.IdPackage.Value)
                .Distinct()
                .ToListAsync();

            // Lấy toàn bộ package
            var allPackages = await _context.Packages
                .AsNoTracking()
                .Include(p => p.PermissionNames)
                .ToListAsync();

            return (allPackages, boughtIds);
        }

        public async Task<PackageDomain> UpdatePackageAsync(PackageDomain packageDomain, Package package, List<string> permissionNames)
        {
            _mapper.Map(packageDomain, package);

            await _context.Entry(package)
                .Collection(p => p.PermissionNames)
                .LoadAsync();
            package.PermissionNames.Clear();

            var newPermissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.PermissionName))
                .ToListAsync();

            foreach (var permission in newPermissions)
                package.PermissionNames.Add(permission);

            await _context.SaveChangesAsync();
            return _mapper.Map<PackageDomain>(package);
        }
    }
}
