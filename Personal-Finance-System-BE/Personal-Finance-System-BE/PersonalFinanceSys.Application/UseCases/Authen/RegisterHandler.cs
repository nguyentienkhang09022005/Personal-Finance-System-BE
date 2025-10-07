using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class RegisterHandler
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<string>> HandleAsync(RegisterRequest registerRequest)
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
                // Tạo user mới
                var newUser = new UserDomain
                (
                    name: registerRequest.Name,
                    email: registerRequest.Email,
                    password: registerRequest.Password
                );

                await _userRepository.AddUserAsync(newUser);

                return ApiResponse<string>.SuccessResponse("Đăng ký thành công!", 200, string.Empty);
            } catch(Exception ex)
            {
                return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
            }
        }
    }
}
