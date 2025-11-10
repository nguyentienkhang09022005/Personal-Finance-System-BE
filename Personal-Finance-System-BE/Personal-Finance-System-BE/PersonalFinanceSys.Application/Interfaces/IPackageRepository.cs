using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IPackageRepository
    {
        Task<PackageDomain> AddPackageAsync(PackageDomain packageDomain, List<string> permissionNames);

        Task<List<PackageDomain>> GetListPackageDomains();

        Task<Package> ExistPackage(Guid idPackage);

        Task<bool> CheckExistPackage(Guid idPackage);

        Task DeletePackageAsync(Guid idPackage);

        Task<PackageDomain> UpdatePackageAsync(PackageDomain packageDomain, Package package, List<string> permissionNames);
    }
}
