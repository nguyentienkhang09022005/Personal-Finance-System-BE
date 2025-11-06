using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<PermissionDomain>> GetListPermissionAsync();
    }
}
