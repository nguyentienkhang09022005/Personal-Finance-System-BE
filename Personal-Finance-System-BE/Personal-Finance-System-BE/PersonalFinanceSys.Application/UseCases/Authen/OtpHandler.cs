using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class OtpHandler
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IFluentEmail _email;
        private readonly IMemoryCache _memoryCache;
        private readonly IUserRepository _userRepository;

        public OtpHandler(PersonFinanceSysDbContext context, 
                          IFluentEmail email, 
                          IMemoryCache memoryCache, 
                          IUserRepository userRepository)
        {
            _context = context;
            _email = email;
            _memoryCache = memoryCache;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<string>> SendOtpToRegisterAsync(RegisterRequest registerRequest)
        {
            // Nếu tồn tại email thì trả về lỗi
            var checkEmail = await _userRepository.GetUserByEmailAsync(registerRequest.Email);
            if (checkEmail != null)
            {
                return ApiResponse<string>.FailResponse("Email đã tồn tại!", 409);
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return ApiResponse<string>.FailResponse("Mật khẩu xác nhận không khớp!", 400);
            }

            try
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var cacheKey = $"OTP_Register_{registerRequest.Email}";
                _memoryCache.Set(cacheKey, new 
                {
                    Otp = otp,
                    registerRequest.Name,
                    registerRequest.Email,
                    registerRequest.Password
                },
                new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) // OTP hết hạn sau 2 phút
                });

                // Gửi OTP đến mail
                var response = await _email
                    .To(registerRequest.Email)
                    .Subject("Mã OTP xác thực đăng ký tài khoản")
                    .Tag("otp-register")
                    .Body($"<p>Mã OTP của bạn là: <strong>{otp}</strong> (hiệu lực trong 2 phút).</p>", true)
                    .SendAsync();

                if (!response.Successful)
                {
                    return ApiResponse<string>.FailResponse("Gửi OTP thất bại!", 500);
                }

                return ApiResponse<string>.SuccessResponse("OTP đã được gửi đến bạn!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
            }
        }

    }
}
