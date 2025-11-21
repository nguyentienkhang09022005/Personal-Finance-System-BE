using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IEvaluateRepository
    {
        Task<EvaluateDomain> AddEvaluateAsync(EvaluateDomain evaluateDomain);

        Task DeleteEvaluteAsync(Guid idEvaluate);

        Task<EvaluateDomain> UpdateEvaluateAsync(EvaluateDomain evaluateDomain, Evaluate evaluate);

        Task<List<EvaluateDomain>> GetListEvaluateByPostIdAsync(Guid idPost);

        Task<bool> ExistEvaluate(Guid idEvaluate);

        Task<Evaluate> GetExistEvaluate(Guid idEvaluate);
    }
}
