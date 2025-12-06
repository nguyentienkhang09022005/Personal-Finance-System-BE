using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Notifications
{
    public class NotificationHandler
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationHubService _notificationHubService;
        private readonly IMapper _mapper;

        public NotificationHandler(INotificationRepository notificationRepository,
                                   IUserRepository userRepository,
                                   INotificationHubService notificationHubService,
                                   IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _notificationHubService = notificationHubService;
        }

        public async Task<ApiResponse<NotificationResponse>> CreateNotificationAsync(NotificationRequest notificationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(notificationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<NotificationResponse>.FailResponse("Không tìm thấy người dùng!", 404);

                var notificationDomain = _mapper.Map<NotificationDomain>(notificationRequest);

                var notificationSaved = await _notificationRepository.AddNotificationAsync(notificationDomain);

                var notificationResponse = _mapper.Map<NotificationResponse>(notificationSaved);
                
                // Gửi websocket
                await _notificationHubService.PushNotificationToUserAsync(notificationSaved.IdUser!.Value, notificationResponse);

                return ApiResponse<NotificationResponse>.SuccessResponse("Tạo bài đăng thành công!", 201, notificationResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<NotificationResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteNotificationAsync(Guid idNotification)
        {
            try
            {
                await _notificationRepository.DeleteNotificationAsync(idNotification);
                return ApiResponse<string>.SuccessResponse("Xóa thông báo thành công!", 200, string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }

        public async Task<ApiResponse<List<NotificationResponse>>> GetListNotificationByUserAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist)
                {
                    return ApiResponse<List<NotificationResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }
                var notificationDomains = await _notificationRepository.GetListNotificationByUserIdAsync(idUser);
                if (!notificationDomains.Any())
                {
                    return ApiResponse<List<NotificationResponse>>.SuccessResponse(
                        "Người dùng chưa thông báo nào!",
                        200,
                        new List<NotificationResponse>());
                }

                var notificationResponses = notificationDomains.Select(notification =>
                {
                    return new NotificationResponse
                    {
                        IdNotification = notification.IdNotification,
                        Title = notification.Title,
                        Content = notification.Content,
                        NotificationType = notification.NotificationType,
                        IsRead = notification.IsRead,
                        NotificationDate = notification.NotificationDate
                    };
                }).ToList();
                return ApiResponse<List<NotificationResponse>>.SuccessResponse(
                    "Lấy danh thông báo của người dùng thành công!", 
                    200, notificationResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<NotificationResponse>>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> UpdateNotificationAsync (Guid idNotification)
        {
            try
            {
                var notificationEntity = await _notificationRepository.GetExistNotificationAsync(idNotification);
                if (notificationEntity == null)
                {
                    return ApiResponse<string>.FailResponse("Không tìm thấy thông báo!", 404);
                }
                var notificationDomain = _mapper.Map<NotificationDomain>(notificationEntity);

                notificationDomain.IsRead = true;

                _mapper.Map(notificationDomain, notificationEntity);

                await _notificationRepository.UpdateNotificationAsync(notificationDomain, notificationEntity);

                return ApiResponse<string>.SuccessResponse(
                    "Cập nhật trạng thái thông báo thành công!",
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
