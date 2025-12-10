using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Notifications;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials
{
    public class MessageHandler
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly NotificationHandler _notificationHandler;
        private readonly IMessageHubService _messageHubService;
        private readonly IMapper _mapper;

        public MessageHandler(IMessageRepository messageRepository,
                              IFriendshipRepository friendshipRepository,
                              IUserRepository userRepository,
                              IImageRepository imageRepository,
                              NotificationHandler notificationHandler,
                              IMessageHubService messageHubService,
                              IMapper mapper)
        {
            _messageRepository = messageRepository;
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _notificationHandler = notificationHandler;
            _messageHubService = messageHubService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MessageResponse>> CreateMessageAsync(MessageRequest messageRequest)
        {
            try
            {
                var friendshipExists = await _friendshipRepository.GetExistFriendship(messageRequest.IdFriendship);
                if (friendshipExists == null)
                    return ApiResponse<MessageResponse>.FailResponse("Không tìm thấy mối quan hệ bạn bè!", 404);

                var userSender = await _userRepository.GetUserByIdAsync(messageRequest.IdUser);
                if (userSender == null)
                    return ApiResponse<MessageResponse>.FailResponse("Không tìm thấy người gửi!", 404);

                var messageDomain = _mapper.Map<MessageDomain>(messageRequest);
                var savedMessage = await _messageRepository.AddMessageAsync(messageDomain);

                Guid idReceiver;
                if (messageRequest.IdUser == friendshipExists.IdUser){
                    idReceiver = friendshipExists.IdRef;
                }
                else{
                    idReceiver = friendshipExists.IdUser;
                }

                var urlAvatar = await _imageRepository.GetImageUrlByIdRefAsync(userSender.IdUser, ConstantTypeRef.TypeUser);

                var messageDetail = new MessageDetailResponse
                {
                    IdMessage = savedMessage.IdMessage,
                    IdSender = userSender.IdUser,
                    Content = savedMessage.Content,
                    SendAt = savedMessage.SendAt
                };

                var messageResponse = new MessageResponse
                {
                    IdUser = userSender.IdUser,
                    Name = userSender.Name,
                    UrlAvatar = urlAvatar,
                    MessageDetailResponses = new List<MessageDetailResponse> { messageDetail }
                };

                // Tạo notification
                var notificationRequest = new NotificationRequest
                {
                    IdUser = idReceiver,
                    IdRelated = savedMessage.IdMessage,
                    NotificationType = ConstantNotificationType.MessageType,
                    Title = "Tin nhắn mới",
                    Content = $"Bạn nhận được tin nhắn mới từ {userSender.Name}.",
                    RelatedType = ConstantNotificationType.MessageType
                };
                await _notificationHandler.CreateNotificationAsync(notificationRequest);

                await _messageHubService.PushMessageToUserAsync(idReceiver, messageResponse);

                return ApiResponse<MessageResponse>.SuccessResponse("Gửi tin nhắn thành công!", 201, messageResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<MessageResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteMessageAsync(Guid idFriendship)
        {
            try
            {
                var friendship = await _friendshipRepository.GetExistFriendship(idFriendship);
                if (friendship == null)
                {
                    return ApiResponse<string>.FailResponse("Không tìm thấy mối quan hệ bạn bè!", 404);
                }

                await _messageRepository.DeleteMessageAsync(idFriendship);
                return ApiResponse<string>.SuccessResponse("Xóa toàn bộ tin nhắn thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<MessageResponse>> GetListMessageAsync(ListMessageRequest listMessageRequest)
        {
            try
            {
                var friendship = await _friendshipRepository.GetExistFriendship(listMessageRequest.IdFriendship);
                if (friendship == null){
                    return ApiResponse<MessageResponse>.FailResponse("Không tìm thấy mối quan hệ bạn bè!", 404);
                }

                Guid idUser = (listMessageRequest.IdUser == friendship.IdUser)
                    ? friendship.IdRef
                    : friendship.IdUser;

                // User
                var userInf = await _userRepository.GetUserByIdAsync(idUser);
                if (userInf == null)
                {
                    return ApiResponse<MessageResponse>.FailResponse("Không tìm thấy thông tin người dùng!", 404);
                }

                var urlAvatar = await _imageRepository.GetImageUrlByIdRefAsync(userInf.IdUser, ConstantTypeRef.TypeUser);

                var messageDomains = await _messageRepository.GetListMessageAsync(listMessageRequest.IdFriendship);

                var messageDetailResponses = messageDomains.Select(message => new MessageDetailResponse
                {
                    IdMessage = message.IdMessage,
                    IdSender = message.IdUser,
                    Content = message.Content,
                    SendAt = message.SendAt
                }).ToList();

                var result = new MessageResponse
                {
                    IdUser = userInf.IdUser,
                    Name = userInf.Name,
                    UrlAvatar = urlAvatar,
                    MessageDetailResponses = messageDetailResponses
                };

                return ApiResponse<MessageResponse>.SuccessResponse("Lấy danh sách tin nhắn thành công!", 200, result);
            }
            catch (Exception ex)
            {
                return ApiResponse<MessageResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }
    }
}
