using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Caching.Memory;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class OtpHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;

        public OtpHandler(IUserRepository userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<ApiResponse<string>> ConfirmOTPForRegisterHandleAsync(ConfirmOTPRequest confirmOTPRequest)
        {
            var cacheKey = $"OTP_Register_{confirmOTPRequest.Email}";
            if (!_memoryCache.TryGetValue<RegisterCacheData>(cacheKey, out var cacheData))
            {
                return ApiResponse<string>.FailResponse("OTP không hợp lệ hoặc đã hết hạn!", 400);
            }    

            // Nếu OTP đúng thì tạo user mới
            if (cacheData.Otp != confirmOTPRequest.OTP)
            {
                return ApiResponse<string>.FailResponse("OTP không đúng!", 400);
            }

            try
            {
                // Tạo user mới
                var newUser = new UserDomain
                (
                    name: cacheData.Name,
                    email: cacheData.Email,
                    password: cacheData.Password
                );

                await _userRepository.AddUserAsync(newUser);

                _memoryCache.Remove(cacheKey);

                return ApiResponse<string>.SuccessResponse("Đăng ký thành công!", 200, string.Empty);
            } catch(Exception ex)
            {
                return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
            }
        }
    }
}
