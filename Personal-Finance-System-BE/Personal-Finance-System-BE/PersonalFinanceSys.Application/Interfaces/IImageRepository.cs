using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IImageRepository
    {
        Task AddImageAsync(ImageDomain imageDomain);

        Task<string?> GetImageUrlByIdRefAsync(Guid idRef, string refType);

        Task<List<string>> GetListImageUrlByIdRefAsync(Guid idRef, string refType);

        Task<Dictionary<Guid, string?>> GetImagesByListRefAsync(IEnumerable<Guid> idRefs, string refType);

        Task DeleteImageByIdRefAsync(Guid idRef, string refType);
    }
}
