using FluentEmail.Core;
using Microsoft.Extensions.Caching.Memory;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using System.Security.Cryptography;
using System.Text;


namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class OtpHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IFluentEmail _email;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<OtpHandler> _logger;


        public OtpHandler(IUserRepository userRepository, IMemoryCache memoryCache, IFluentEmail email, ILogger<OtpHandler> logger)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _email = email;
            _logger = logger;
        }

        // Hàm gửi OTP cho quên mật khẩu
        public async Task<ApiResponse<string>> SendOTPForForgotPasswordHandleAsync(ForgotPasswordRequest forgotPasswordRequest)
        {
            // Nếu không tồn tại email thì trả về lỗi
            var checkEmail = await _userRepository.GetUserByEmailAsync(forgotPasswordRequest.Email);
            if (checkEmail == null)
            {
                return ApiResponse<string>.FailResponse("Email không tồn tại!", 404);
            }
            try
            {
                var otp = new Random().Next(100000, 999999).ToString();
                var cacheKey = $"OTP_ForgotPassword_{forgotPasswordRequest.Email}";
                var cacheData = new ForgotPasswordCacheData
                {
                    Otp = otp,
                    Email = forgotPasswordRequest.Email
                };
                _memoryCache.Set(cacheKey, cacheData, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) // OTP hết hạn sau 2 phút
                });

                var response = await _email
                    .To(forgotPasswordRequest.Email)
                    .Subject("Mã OTP hỗ trợ quên mật khẩu")
                    .Tag("otp-forgot-password")
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

        // Hàm xử lý xác thực OTP cho đăng ký
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

        // Hàm xử lý xác thực OTP cho quên mật khẩu và đổi mật khẩu mới
        public async Task<ApiResponse<string>> ConfirmOTPForForgotPasswordHandleAsync(ChangePasswordRequest changePasswordRequest)
        {
            var cacheKey = $"OTP_ForgotPassword_{changePasswordRequest.Email}";
            if (!_memoryCache.TryGetValue<ForgotPasswordCacheData>(cacheKey, out var cacheData))
            {
                return ApiResponse<string>.FailResponse("OTP không hợp lệ hoặc đã hết hạn!", 400);
            }
            // Nếu OTP đúng thì đổi mật khẩu
            if (cacheData.Otp != changePasswordRequest.OTP)
            {
                return ApiResponse<string>.FailResponse("OTP không đúng!", 400);
            }
            if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
            {
                return ApiResponse<string>.FailResponse("Mật khẩu xác nhận không khớp!", 400);
            }
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(changePasswordRequest.Email);
                if (user == null)
                {
                    return ApiResponse<string>.FailResponse("Email không tồn tại!", 404);
                }
                user.SetPassword(BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword));
                await _userRepository.UpdateUserAsync(user);

                _memoryCache.Remove(cacheKey);
                return ApiResponse<string>.SuccessResponse("Đổi mật khẩu thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
            }
        }
    }
}
