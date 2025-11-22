using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials
{
    public class FriendshipHandler
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FriendshipHandler(IFriendshipRepository friendshipRepository,
                                 IUserRepository userRepository,
                                 IMapper mapper)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateFriendshipAsync(FriendshipCreationRequest friendshipCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(friendshipCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người gửi!", 404);

                bool refExists = await _userRepository.ExistUserAsync(friendshipCreationRequest.IdRef);
                if (!refExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người nhận!", 404);

                var friendshipDomain = _mapper.Map<FriendshipDomain>(friendshipCreationRequest);
                await _friendshipRepository.AddFriendshipAsync(friendshipDomain);

                return ApiResponse<string>.SuccessResponse("Kết bạn thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteFriendshipAsync(Guid idFriendship)
        {
            try
            {
                await _friendshipRepository.DeleteFriendshipAsync(idFriendship);
                return ApiResponse<string>.SuccessResponse("Xóa kết bạn thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> AcceptFriendshipAsync(Guid idFriendship, FriendshipUpdateRequest friendshipUpdateRequest)
        {
            try
            {
                var friendshipEntity = await _friendshipRepository.GetExistFriendship(idFriendship);
                if (friendshipEntity == null)
                {
                    return ApiResponse<string>.FailResponse("Không tìm thấy mục bạn bè!", 404);
                }

                var friendshipDomain = _mapper.Map<FriendshipDomain>(friendshipEntity);

                _mapper.Map(friendshipUpdateRequest, friendshipDomain);

                await _friendshipRepository.AcceptFriendshipAsync(friendshipDomain, friendshipEntity);

                return ApiResponse<string>.SuccessResponse("Kết bạn thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> RejectFriendshipAsync(Guid idFriendship, FriendshipUpdateRequest friendshipUpdateRequest)
        {
            try
            {
                var friendshipEntity = await _friendshipRepository.GetExistFriendship(idFriendship);
                if (friendshipEntity == null)
                {
                    return ApiResponse<string>.FailResponse("Không tìm thấy mục bạn bè!", 404);
                }

                var friendshipDomain = _mapper.Map<FriendshipDomain>(friendshipEntity);

                _mapper.Map(friendshipUpdateRequest, friendshipDomain);

                await _friendshipRepository.RejectFriendshipAsync(friendshipDomain, friendshipEntity);

                return ApiResponse<string>.SuccessResponse("Từ chối kết bạn thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }
    }
}
