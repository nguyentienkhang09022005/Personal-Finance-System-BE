using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IUpLoadImageFileService
    {
        Task<ApiResponse<string>> UploadImageAsync(IFormFile file);
    }
}
