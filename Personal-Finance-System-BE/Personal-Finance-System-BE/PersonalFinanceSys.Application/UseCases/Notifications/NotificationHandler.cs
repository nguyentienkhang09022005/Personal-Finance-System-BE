using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid;

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
    }
}
