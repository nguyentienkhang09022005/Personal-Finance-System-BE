using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using System.Text.RegularExpressions;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class UserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Lấy danh sách người dùng
        public async Task<ApiResponse<List<UserResponse>>> GetListUserHandleAsync()
        {
            var userDomains = await _userRepository.GetListUserAsync();
            var userResponse = _mapper.Map<List<UserResponse>>(userDomains);
            return ApiResponse<List<UserResponse>>.SuccessResponse("Lấy danh sách người dùng thành công", 200, userResponse);
        }

        // Hàm lấy thông tin chi tiết người dùng qua ID
        public async Task<ApiResponse<UserResponse>> GetInfUserHandleAsync(Guid idUser)
        {
            var userDomain = await _userRepository.GetUserByIdAsync(idUser);
            var userResponse = _mapper.Map<UserResponse>(userDomain);
            return ApiResponse<UserResponse>.SuccessResponse("Lấy thông tin người dùng thành công", 200, userResponse);
        }

        // Tạo người dùng mới
        public async Task<ApiResponse<string>> CreateUserHandleAsync(UserRequest userRequest)
        {
            if (userRequest == null)
            {
                return ApiResponse<string>.FailResponse("Lỗi chưa gửi request!", 500);
            }

            // Kiểm tra định dạng email
            if (string.IsNullOrWhiteSpace(userRequest.Email) ||
                    !Regex.IsMatch(userRequest.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return ApiResponse<string>.FailResponse("Email không hợp lệ! Phải có dạng example@gmail.com", 400);
            }

            // Kiểm tra định dạng sđt
            if (string.IsNullOrWhiteSpace(userRequest.Phone) || !Regex.IsMatch(userRequest.Phone, @"^[0-9]{10,12}$"))
            {
                return ApiResponse<string>.FailResponse("Số điện thoại phải có từ 10 đến 12 chữ số!", 400);
            }

            // Kiểm tra password và confirm password
            if (string.IsNullOrWhiteSpace(userRequest.Password) ||
                string.IsNullOrWhiteSpace(userRequest.ConfirmPassword))
            {
                return ApiResponse<string>.FailResponse("Mật khẩu không được để trống!", 400);
            }

            if (userRequest.Password != userRequest.ConfirmPassword)
            {
                return ApiResponse<string>.FailResponse("Mật khẩu và xác nhận mật khẩu không trùng khớp!", 400);
            }

            var newUser = _mapper.Map<UserDomain>(userRequest);
            await _userRepository.AddUserAsync(newUser);
            return ApiResponse<string>.SuccessResponse("Tạo người dùng thành công!", 200, string.Empty);
        }

        //public async Task<ApiResponse<UserResponse?>> updateUserHandleAsync(UserRequest userRequest)

        // Hàm xóa tài khoản người dùng
        public async Task<ApiResponse<string>> deleteUserHandleAsync(Guid idUser)
        {
            await _userRepository.DeleteUserAsync(idUser);
            return ApiResponse<string>.SuccessResponse("Xóa người dùng thành công!", 200, string.Empty);
        }
    }
}
