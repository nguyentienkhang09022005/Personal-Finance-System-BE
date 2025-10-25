using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class RegisterHandler
    {
        private readonly IFluentEmail _email;
        private readonly IMemoryCache _memoryCache;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OtpHandler> _logger;


        public RegisterHandler(IFluentEmail email,
                          IMemoryCache memoryCache,
                          IUserRepository userRepository,
                          ILogger<OtpHandler> logger)
        {
            _email = email;
            _memoryCache = memoryCache;
            _userRepository = userRepository;
            _logger = logger;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> SendOtpToRegisterAsync(RegisterRequest registerRequest)
        {
            // Nếu tồn tại email thì trả về lỗi
            var checkEmail = await _userRepository.GetUserByEmailAsync(registerRequest.Email);
            if (checkEmail != null){
                return ApiResponse<string>.FailResponse("Email đã tồn tại!", 409);
            }

            if (string.IsNullOrWhiteSpace(registerRequest.Email) || !registerRequest.Email.Contains("@")){
                return ApiResponse<string>.FailResponse("Email không hợp lệ!", 400);
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword){
                return ApiResponse<string>.FailResponse("Mật khẩu xác nhận không khớp!", 400);
            }

            try
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var cacheKey = $"OTP_Register_{registerRequest.Email}";

                var cacheData = new RegisterCacheData
                {
                    Otp = otp,
                    Name = registerRequest.Name,
                    Email = registerRequest.Email,
                    Password = registerRequest.Password
                };

                _memoryCache.Set(cacheKey, cacheData , new MemoryCacheEntryOptions()
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
