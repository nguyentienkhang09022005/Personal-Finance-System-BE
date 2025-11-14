using AutoMapper;
using FluentEmail.Core;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Users
{
    public class UserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        
        public UserHandler(IUserRepository userRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        // Lấy danh sách người dùng
        public async Task<ApiResponse<List<UserResponse>>> GetListUserHandleAsync()
        {
            try
            {
                var userDomains = await _userRepository.GetListUserAsync();
                if (userDomains == null || !userDomains.Any())
                    return ApiResponse<List<UserResponse>>.SuccessResponse("Không có người dùng nào", 200, new List<UserResponse>());

                var idUsers = userDomains.Select(u => u.IdUser).ToList();

                var imageDict = await _imageRepository.GetImagesByListRefAsync(idUsers, "USERS");

                var userResponses = _mapper.Map<List<UserResponse>>(userDomains);

                foreach (var userResponse in userResponses)
                {
                    imageDict.TryGetValue(userResponse.IdUser, out var urlAvatar);
                    userResponse.UrlAvatar = string.IsNullOrEmpty(urlAvatar) ? null : urlAvatar;
                }

                return ApiResponse<List<UserResponse>>.SuccessResponse("Lấy danh sách người dùng thành công", 200, userResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserResponse>>.FailResponse($"Lỗi khi lấy danh sách người dùng: {ex.Message}", 500);
            }
        }


        // Hàm lấy thông tin chi tiết người dùng qua ID
        public async Task<ApiResponse<UserResponse>> GetInfUserHandleAsync(Guid idUser)
        {
            try
            {
                // Song song lấy thông tin người dùng và ảnh đại diện
                var userTask = _userRepository.GetUserByIdAsync(idUser);
                var imageTask = _imageRepository.GetImageUrlByIdRefAsync(idUser, "USERS");

                await Task.WhenAll(userTask, imageTask);

                var userDomain = await userTask;
                var urlAvatar = await imageTask;

                var userResponse = _mapper.Map<UserResponse>(userDomain);
                userResponse.UrlAvatar = string.IsNullOrEmpty(urlAvatar) ? null : urlAvatar;

                return ApiResponse<UserResponse>.SuccessResponse(
                    "Lấy thông tin người dùng thành công",
                    200,
                    userResponse
                );
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<UserResponse>.FailResponse(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.FailResponse($"Lỗi khi lấy thông tin: {ex.Message}", 500);
            }
        }


        // Tạo người dùng mới
        public async Task<ApiResponse<string>> CreateUserHandleAsync(UserCreationRequest userCreationRequest)
        {
            try
            {
                if (userCreationRequest == null){
                    return ApiResponse<string>.FailResponse("Lỗi chưa gửi request!", 500);
                }

                // Kiểm tra password và confirm password
                if (string.IsNullOrWhiteSpace(userCreationRequest.Password) ||
                    string.IsNullOrWhiteSpace(userCreationRequest.ConfirmPassword))
                {
                    return ApiResponse<string>.FailResponse("Mật khẩu không được để trống!", 400);
                }

                if (userCreationRequest.Password != userCreationRequest.ConfirmPassword)
                {
                    return ApiResponse<string>.FailResponse(
                        "Mật khẩu và xác nhận mật khẩu không trùng khớp!", 
                        400);
                }

                var newUser = _mapper.Map<UserDomain>(userCreationRequest);
                var createdUser = await _userRepository.AddUserAsync(newUser);

                if (userCreationRequest.UrlAvatar != null)
                {
                    var avartar = new ImageDomain
                    {
                        Url = userCreationRequest.UrlAvatar,
                        IdRef = createdUser.IdUser,
                        RefType = ConstantRole.UserRole 
                    };
                    await _imageRepository.AddImageAsync(avartar);
                }
                return ApiResponse<string>.SuccessResponse(
                    "Tạo người dùng thành công!", 
                    200, 
                    string.Empty);
            }
            catch (Exception ex){
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }   
        }

        public async Task<ApiResponse<UserResponse>> UpdateUserHandleAsync(Guid idUser, UserUpdateRequest userUpdateRequest)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userUpdateRequest.Phone))
                {
                    if (userUpdateRequest.Phone.Length < 10 || userUpdateRequest.Phone.Length > 12)
                        return ApiResponse<UserResponse>.FailResponse("Số điện thoại không hợp lệ!", 400);
                }

                if (!string.IsNullOrWhiteSpace(userUpdateRequest.Email))
                {
                    if (!userUpdateRequest.Email.Contains("@"))
                        throw new ArgumentException("Email không hợp lệ! Phải có dạng example@gmail.com!");
                }
                    
                var userEntity = await _userRepository.GetExistUserAsync(idUser);
                if (userEntity == null){
                    return ApiResponse<UserResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var userDomain = _mapper.Map<UserDomain>(userEntity);

                _mapper.Map(userUpdateRequest, userDomain);

                // Cập nhật ảnh đại diện nếu có
                if (userUpdateRequest.UrlAvatar != null)
                {
                    var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idUser, "USERS");
                    if (!string.IsNullOrEmpty(existingImageUrl))
                    {
                        await _imageRepository.DeleteImageByIdRefAsync(idUser, "USERS");
                    }

                    var image = new ImageDomain
                    {
                        Url = userUpdateRequest.UrlAvatar,
                        IdRef = idUser,
                        RefType = ConstantRole.UserRole
                    };

                    await _imageRepository.AddImageAsync(image);
                }
                var updatedUser = await _userRepository.UpdateUserAsync(userDomain, userEntity);

                var userResponse = _mapper.Map<UserResponse>(updatedUser);
                return ApiResponse<UserResponse>.SuccessResponse(
                    "Cập nhật người dùng thành công!", 
                    200, 
                    userResponse);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<UserResponse>.FailResponse(ex.Message, 404);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.FailResponse($"Lỗi khi cập nhật người dùng: {ex.Message}", 500);
            }
        }

        // Hàm xóa tài khoản người dùng
        public async Task<ApiResponse<string>> deleteUserHandleAsync(Guid idUser)
        {
            try
            {
                await _userRepository.DeleteUserAsync(idUser);
                var existingAvatarUrl = await _imageRepository.GetImageUrlByIdRefAsync(idUser, "USERS");
                if (!string.IsNullOrEmpty(existingAvatarUrl)){
                    await _imageRepository.DeleteImageByIdRefAsync(idUser, "USERS");
                }
                
                return ApiResponse<string>.SuccessResponse(
                    "Xóa người dùng thành công!", 
                    200, 
                    string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }
    }
}
