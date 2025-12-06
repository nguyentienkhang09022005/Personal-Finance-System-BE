using AutoMapper;
using Npgsql;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Notifications;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials
{
    public class FriendshipHandler
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly INotificationHubService _notificationHubService;
        private readonly NotificationHandler _notificationHandler;
        private readonly IMapper _mapper;

        public FriendshipHandler(IFriendshipRepository friendshipRepository,
                                 IUserRepository userRepository,
                                 IImageRepository imageRepository,
                                 INotificationHubService notificationHubService,
                                 NotificationHandler notificationHandler,
                                 IMapper mapper)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _notificationHubService = notificationHubService;
            _notificationHandler = notificationHandler;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<FriendshipResponse>>> GetListGetListFriendshipOfUserAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist){
                    return ApiResponse<List<FriendshipResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }
                var friendshipDomains = await _friendshipRepository.GetListFriendshipOfUserAsync(idUser);
                if (!friendshipDomains.Any())
                {
                    return ApiResponse<List<FriendshipResponse>>.SuccessResponse(
                        "Danh sách bạn bè trống!",
                        200,
                        new List<FriendshipResponse>());
                }

                var allIdUsers = friendshipDomains
                    .SelectMany(f => new List<Guid> { f.IdUser, f.IdRef })
                    .Distinct()
                    .ToList();

                var avatarUrls = await _imageRepository.GetImagesByListRefAsync(allIdUsers, ConstantTypeRef.TypeUser);

                var friendshipResponses = new List<FriendshipResponse>();

                foreach (var friendshipDomain in friendshipDomains)
                {
                    var friendshipResponse = _mapper.Map<FriendshipResponse>(friendshipDomain);

                    if (friendshipResponse.infFriendshipResponse.Sender == null)
                        friendshipResponse.infFriendshipResponse.Sender = new InfFriendOfFriendshipResponse();

                    if (friendshipResponse.infFriendshipResponse.Receiver == null)
                        friendshipResponse.infFriendshipResponse.Receiver = new InfFriendOfFriendshipResponse();
                    
                    if (avatarUrls.TryGetValue(friendshipDomain.IdUser, out var senderAvatar))
                        friendshipResponse.infFriendshipResponse.Sender.UrlAvatar = senderAvatar;

                    if (avatarUrls.TryGetValue(friendshipDomain.IdRef, out var receiverAvatar))
                        friendshipResponse.infFriendshipResponse.Receiver.UrlAvatar = receiverAvatar;

                    friendshipResponses.Add(friendshipResponse);
                }
                return ApiResponse<List<FriendshipResponse>>.SuccessResponse("Lấy danh sách bạn bè thành công!", 200, friendshipResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<FriendshipResponse>>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<FriendshipResponse>>> GetListFriendshipSentWithStatusPendingByUserAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist)
                {
                    return ApiResponse<List<FriendshipResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }
                var friendshipDomains = await _friendshipRepository.GetListFriendshipSentWithStatusPendingByUserAsync(idUser);
                if (!friendshipDomains.Any())
                {
                    return ApiResponse<List<FriendshipResponse>>.SuccessResponse(
                        "Danh sách lời mời đã gửi trống!",
                        200,
                        new List<FriendshipResponse>());
                }

                var allIdUsers = friendshipDomains
                    .SelectMany(f => new List<Guid> { f.IdUser, f.IdRef })
                    .Distinct()
                    .ToList();

                var avatarUrls = await _imageRepository.GetImagesByListRefAsync(allIdUsers, ConstantTypeRef.TypeUser);

                var friendshipResponses = new List<FriendshipResponse>();

                foreach (var friendshipDomain in friendshipDomains)
                {
                    var friendshipResponse = _mapper.Map<FriendshipResponse>(friendshipDomain);

                    if (friendshipResponse.infFriendshipResponse.Sender == null)
                        friendshipResponse.infFriendshipResponse.Sender = new InfFriendOfFriendshipResponse();

                    if (friendshipResponse.infFriendshipResponse.Receiver == null)
                        friendshipResponse.infFriendshipResponse.Receiver = new InfFriendOfFriendshipResponse();

                    if (avatarUrls.TryGetValue(friendshipDomain.IdUser, out var senderAvatar))
                        friendshipResponse.infFriendshipResponse.Sender.UrlAvatar = senderAvatar;

                    if (avatarUrls.TryGetValue(friendshipDomain.IdRef, out var receiverAvatar))
                        friendshipResponse.infFriendshipResponse.Receiver.UrlAvatar = receiverAvatar;

                    friendshipResponses.Add(friendshipResponse);
                }

                return ApiResponse<List<FriendshipResponse>>.SuccessResponse("Lấy danh sách lời mời đã gửi thành công!", 200, friendshipResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<FriendshipResponse>>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<FriendshipResponse>>> GetListFriendshipReceivedWithStatusPendingByUserAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist)
                {
                    return ApiResponse<List<FriendshipResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }
                var friendshipDomains = await _friendshipRepository.GetListFriendshipReceivedWithStatusPendingByUserAsync(idUser);
                if (!friendshipDomains.Any())
                {
                    return ApiResponse<List<FriendshipResponse>>.SuccessResponse(
                        "Danh sách lời mời kết bạn trống!",
                        200,
                        new List<FriendshipResponse>());
                }

                var allIdUsers = friendshipDomains
                    .SelectMany(f => new List<Guid> { f.IdUser, f.IdRef })
                    .Distinct()
                    .ToList();

                var avatarUrls = await _imageRepository.GetImagesByListRefAsync(allIdUsers, ConstantTypeRef.TypeUser);

                var friendshipResponses = new List<FriendshipResponse>();

                foreach (var friendshipDomain in friendshipDomains)
                {
                    var friendshipResponse = _mapper.Map<FriendshipResponse>(friendshipDomain);

                    if (friendshipResponse.infFriendshipResponse.Sender == null)
                        friendshipResponse.infFriendshipResponse.Sender = new InfFriendOfFriendshipResponse();

                    if (friendshipResponse.infFriendshipResponse.Receiver == null)
                        friendshipResponse.infFriendshipResponse.Receiver = new InfFriendOfFriendshipResponse();

                    if (avatarUrls.TryGetValue(friendshipDomain.IdUser, out var senderAvatar))
                        friendshipResponse.infFriendshipResponse.Sender.UrlAvatar = senderAvatar;

                    if (avatarUrls.TryGetValue(friendshipDomain.IdRef, out var receiverAvatar))
                        friendshipResponse.infFriendshipResponse.Receiver.UrlAvatar = receiverAvatar;

                    friendshipResponses.Add(friendshipResponse);
                }
                return ApiResponse<List<FriendshipResponse>>.SuccessResponse("Lấy danh sách lời mời kết bạn thành công!", 200, friendshipResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<FriendshipResponse>>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> CreateFriendshipAsync(FriendshipCreationRequest friendshipCreationRequest)
        {
            try
            {
                var userExists = await _userRepository.GetExistUserAsync(friendshipCreationRequest.IdUser);
                if (userExists == null)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người gửi!", 404);

                bool refExists = await _userRepository.ExistUserAsync(friendshipCreationRequest.IdRef);
                if (!refExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người nhận!", 404);

                bool friendshipExists = await _friendshipRepository.ExistFriendship(friendshipCreationRequest.IdRef, friendshipCreationRequest.IdUser);
                if (friendshipExists)
                    return ApiResponse<string>.FailResponse("Đã kết bạn với người này rồi!", 400);

                var friendshipDomain = _mapper.Map<FriendshipDomain>(friendshipCreationRequest);
                await _friendshipRepository.AddFriendshipAsync(friendshipDomain);

                var notificationRequest = new NotificationRequest
                {
                    IdUser = friendshipCreationRequest.IdRef,
                    IdRelated = friendshipCreationRequest.IdUser,
                    NotificationType = ConstantNotificationType.FriendshipType,
                    Title = "Lời mời kết bạn mới!",
                    Content = $"Bạn nhận được lời mời kết bạn từ người dùng {userExists.Name}.",
                    RelatedType = ConstantNotificationType.FriendshipType,
                };

                await _notificationHandler.CreateNotificationAsync(notificationRequest);

                return ApiResponse<string>.SuccessResponse("Gửi lời mời kết bạn thành công!", 201, string.Empty);
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

                var notificationRequest = new NotificationRequest
                {
                    IdUser = friendshipDomain.IdUser,
                    IdRelated = friendshipDomain.IdRef,
                    NotificationType = ConstantNotificationType.FriendshipType,
                    Title = "Lời mời được chấp nhận",
                    Content = $"Lời mời kết bạn của bạn đã được chấp nhận bởi người dùng {friendshipDomain.IdRef}.",
                    RelatedType = ConstantNotificationType.FriendshipType,
                };

                await _notificationHandler.CreateNotificationAsync(notificationRequest);

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
