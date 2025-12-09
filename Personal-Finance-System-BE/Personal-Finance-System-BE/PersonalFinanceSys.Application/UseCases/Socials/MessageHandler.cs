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

        public async Task<ApiResponse<string>> DeleteMessageAsync(Guid idMessage)
        {
            try
            {
                await _messageRepository.DeleteMessageAsync(idMessage);
                return ApiResponse<string>.SuccessResponse("Xóa tin nhắn thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<ListMessageResponse>> GetListMessageAsync(ListMessageRequest listMessageRequest)
        {
            try
            {
                var friendship = await _friendshipRepository.GetExistFriendship(listMessageRequest.IdFriendship);
                if (friendship == null){
                    throw new Exception("Không tìm thấy mối quan hệ bạn bè!");
                }

                Guid idUser = listMessageRequest.IdUser; 
                
                Guid idRef = (idUser == friendship.IdUser)
                    ? friendship.IdRef
                    : friendship.IdUser;

                // User
                var userInf = await _userRepository.GetUserByIdAsync(idUser);
                if (userInf == null)
                {
                    return ApiResponse<ListMessageResponse>.FailResponse("Không tìm thấy thông tin người dùng!", 404);
                }

                var urlAvatarUser = await _imageRepository.GetImageUrlByIdRefAsync(userInf.IdUser, ConstantTypeRef.TypeUser);

                // Ref
                var refInf = await _userRepository.GetUserByIdAsync(idRef);
                if (refInf == null)
                {
                    return ApiResponse<ListMessageResponse>.FailResponse("Không tìm thấy thông tin bạn bè!", 404);
                }

                var urlAvatarRef = await _imageRepository.GetImageUrlByIdRefAsync(refInf.IdUser, ConstantTypeRef.TypeUser);

                var messageDomains = await _messageRepository.GetListMessageAsync(listMessageRequest.IdFriendship);

                var messageDetailResponses = messageDomains.Select(message => new MessageDetailResponse
                {
                    IdMessage = message.IdMessage,
                    Content = message.Content,
                    SendAt = message.SendAt
                }).ToList();

                var result = new ListMessageResponse
                {
                    IdUser = userInf.IdUser,
                    NameUser = userInf.Name,
                    UrlAvatarUser = urlAvatarRef,
                    IdRef = refInf.IdUser,
                    NameRef = refInf.Name,
                    UrlAvatarRef = urlAvatarRef,
                    MessageDetailResponses = messageDetailResponses
                };

                return ApiResponse<ListMessageResponse>.SuccessResponse("Lấy danh sách tin nhắn thành công!", 200, result);
            }
            catch (Exception ex)
            {
                return ApiResponse<ListMessageResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }
    }
}
