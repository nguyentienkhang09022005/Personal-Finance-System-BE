using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class UpLoadImageFileService : IUpLoadImageFileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<UpLoadImageFileService> _logger;

        public UpLoadImageFileService(Cloudinary cloudinary, ILogger<UpLoadImageFileService> logger)
        {
            _cloudinary = cloudinary;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> UploadImageAsync(IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
                throw new ArgumentException("Invalid file");

            var publicId = GeneratePublicValue(file.FileName);
            _logger.LogInformation("publicId is: {PublicId}", publicId);

            var extension = Path.GetExtension(file.FileName);
            _logger.LogInformation("extension is: {Extension}", extension);

            var tempPath = Path.GetTempFileName() + extension;

            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(tempPath),
                PublicId = publicId
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            File.Delete(tempPath); // Clean up temp file

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return ApiResponse<string>.SuccessResponse("Upload hình ảnh lên server thành công!", 201, _cloudinary.Api.UrlImgUp.BuildUrl($"{publicId}{extension}"));
            }

            throw new Exception("Image upload failed.");
        }

        private string GeneratePublicValue(string originalName)
        {
            var fileName = Path.GetFileNameWithoutExtension(originalName);
            return $"{Guid.NewGuid()}_{fileName}";
        }
    }
}
