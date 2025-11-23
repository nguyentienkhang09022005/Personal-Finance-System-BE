using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials
{
    public class MessageHandler
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public MessageHandler(IMessageRepository messageRepository,
                              IFriendshipRepository friendshipRepository,
                              IUserRepository userRepository,
                              IImageRepository imageRepository,
                              IMapper mapper)
        {
            _messageRepository = messageRepository;
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateMessageAsync(MessageRequest messageRequest)
        {
            try
            {
                bool friendshipExists = await _friendshipRepository.ExistFriendship(messageRequest.IdFriendship);
                if (!friendshipExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy mối quan hệ bạn bè!", 404);

                var friendship = await _friendshipRepository.GetExistFriendship(messageRequest.IdFriendship);

                var messageDomain = _mapper.Map<MessageDomain>(messageRequest);
                await _messageRepository.AddMessageAsync(messageDomain);

                return ApiResponse<string>.SuccessResponse("Gửi tin nhắn thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
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

        public async Task<ApiResponse<MessageResponse>> GetListMessageAsync(Guid idFriendship)
        {
            try
            {
                var checkFriendshipExist = await _friendshipRepository.GetExistFriendship(idFriendship);
                if (checkFriendshipExist == null){
                    throw new Exception("Không tìm thấy mối quan hệ bạn bè!");
                }

                var infRefUser = await _userRepository.GetUserByIdAsync(checkFriendshipExist.IdRef);
                if (infRefUser == null){
                    throw new Exception("Không tìm thấy thông tin người bạn!");
                }

                await _messageRepository.UpdateStatusMessageAsync(idFriendship);

                var messageDomains = await _messageRepository.GetListMessageAsync(idFriendship);

                var UrlAvatar = await _imageRepository.GetImageUrlByIdRefAsync(infRefUser.IdUser, ConstantTypeRef.TypeUser);

                var messageDetailResponse = messageDomains.Select(message => new MessageDetailResponse
                {
                    IdMessage = message.IdMessage,
                    Content = message.Content,
                    IsFriend = message.IsFriend,
                    IsRead = message.IsRead,
                    SendAt = message.SendAt
                }).ToList();

                var result = new MessageResponse
                {
                    IdUser = infRefUser.IdUser,
                    Name = infRefUser.Name,
                    UrlAvatar = UrlAvatar,
                    MessageDetailResponses = messageDetailResponse
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
