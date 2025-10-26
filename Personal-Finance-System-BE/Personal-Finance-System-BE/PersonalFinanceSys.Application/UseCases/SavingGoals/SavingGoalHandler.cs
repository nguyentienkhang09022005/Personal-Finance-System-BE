using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.SavingGoals
{
    public class SavingGoalHandler
    {
        private readonly ISavingGoalRepository _savingGoalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public SavingGoalHandler(ISavingGoalRepository savingGoalRepository,
                                 IUserRepository userRepository,
                                 IImageRepository imageRepository,
                                 IMapper mapper)
        {
            _savingGoalRepository = savingGoalRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateSavingGoalAsync(SavingGoalCreationRequest savingGoalCreationRequest)
        {
            try
            {
                var savingGoalDomain = _mapper.Map<SavingGoalDomain>(savingGoalCreationRequest);
                var createdSavingGoal = await _savingGoalRepository.AddSavingGoalAsync(savingGoalDomain);

                if (savingGoalCreationRequest.UrlImage != null)
                {
                    var image = new ImageDomain
                    {
                        Url = savingGoalCreationRequest.UrlImage,
                        IdRef = createdSavingGoal.IdSaving,
                        RefType = "SAVING_GOAL"
                    };
                    await _imageRepository.AddImageAsync(image);
                }
                return ApiResponse<string>.SuccessResponse("Tạo mục tiêu tiết kiệm thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteSavingGoalAsync(Guid idSavingGoal)
        {
            try
            {
                await _savingGoalRepository.DeleteSavingGoalAsync(idSavingGoal);
                var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idSavingGoal, "SAVING_GOAL");
                if (!string.IsNullOrEmpty(existingImageUrl)){
                    await _imageRepository.DeleteImageByIdRefAsync(idSavingGoal, "SAVING_GOAL");
                }

                return ApiResponse<string>.SuccessResponse(
                    "Xóa mục tiêu tiết kiệm thành công!", 
                    200, 
                    string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<SavingGoalResponse>> UpdateSavingGoalAsync(Guid idSavingGoal, SavingGoalUpdateRequest savingGoalUpdateRequest)
        {
            try
            {
                var savingGoalEntity = await _savingGoalRepository.GetExistSavingGoal(idSavingGoal);
                if (savingGoalEntity == null)
                {
                    return ApiResponse<SavingGoalResponse>.FailResponse("Không tìm thấy mục tiêu tiết kiệm!", 404);
                }
                var savingGoalDomain = _mapper.Map<SavingGoalDomain>(savingGoalEntity);

                _mapper.Map(savingGoalUpdateRequest, savingGoalDomain);

                // Cập nhật ảnh nếu có
                if (savingGoalUpdateRequest.UrlImage != null)
                {
                    var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idSavingGoal, "SAVING_GOAL");
                    if (!string.IsNullOrEmpty(existingImageUrl)){
                        await _imageRepository.DeleteImageByIdRefAsync(idSavingGoal, "SAVING_GOAL");
                    }

                    var image = new ImageDomain
                    {
                        Url = savingGoalUpdateRequest.UrlImage,
                        IdRef = idSavingGoal,
                        RefType = "SAVING_GOAL"
                    };

                    await _imageRepository.AddImageAsync(image);
                }

                var updatedSavingGoal = await _savingGoalRepository.UpdateSavingGoalAsync(savingGoalDomain, savingGoalEntity);

                var savingGoalResponse = _mapper.Map<SavingGoalResponse>(updatedSavingGoal);
                return ApiResponse<SavingGoalResponse>.SuccessResponse("Thay đổi thông tin mục tiêu tiết kiệm thành công!", 200, savingGoalResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<SavingGoalResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<SavingGoalResponse>>> GetListSavingGoalAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist){
                    return ApiResponse<List<SavingGoalResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var savingGoalDomains = await _savingGoalRepository.GetListSavingGoalsAsync(idUser);
                if (!savingGoalDomains.Any()){
                    return ApiResponse<List<SavingGoalResponse>>.SuccessResponse(
                        "Người dùng chưa có mục tiêu tiết kiệm nào!", 
                        200, 
                        new List<SavingGoalResponse>());
                }

                // Lấy danh sách id mục tiêu tiết kiệm để truy xuất ảnh
                var idRefs = savingGoalDomains.Select(g => g.IdSaving).ToList();
                var imageDict = await _imageRepository.GetImagesByListRefAsync(idRefs, "SAVING_GOAL");

                var savingGoalResponses = savingGoalDomains.Select(goal =>
                {
                    var currentAmount = goal.SavingDetails.Sum(d => d.Amount ?? 0); // Tổng tiền đã tiết kiệm
                    var remaining = goal.TargetAmount - currentAmount; // Số tiền còn lại cần tiết kiệm
                    var progress = goal.TargetAmount > 0 // Tiến độ tiết kiệm (%)
                        ? Math.Round((double)(currentAmount / goal.TargetAmount) * 100, 2) : 0;

                    return new SavingGoalResponse
                    {
                        IdSaving = goal.IdSaving,
                        SavingName = goal.SavingName,
                        TargetAmount = goal.TargetAmount,
                        CurrentAmount = currentAmount,
                        RemainingAmount = remaining < 0 ? 0 : remaining,
                        SavingProgress = progress,
                        TargetDate = goal.TargetDate,
                        Status = goal.Status,
                        Description = goal.Description,
                        CreateAt = goal.CreateAt,
                        UrlImage = imageDict.ContainsKey(goal.IdSaving) ? imageDict[goal.IdSaving] : null
                    };
                }).ToList();

                return ApiResponse<List<SavingGoalResponse>>.SuccessResponse("Lấy danh sách mục tiêu tiết kiệm thành công!", 200, savingGoalResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SavingGoalResponse>>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<SavingGoalResponse>> GetSavingGoalByIdAsync(Guid idSavingGoal)
        {
            try
            {
                var savingGoalDomain = await _savingGoalRepository.GetSavingGoalByIdAsync(idSavingGoal);
                if (savingGoalDomain == null){
                    return ApiResponse<SavingGoalResponse>.FailResponse("Không tìm thấy mục tiêu tiết kiệm!", 404);
                }

                var imageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idSavingGoal, "SAVING_GOAL");

                var currentAmount = savingGoalDomain.SavingDetails.Sum(d => d.Amount ?? 0); // Tổng tiền đã tiết kiệm
                var remaining = savingGoalDomain.TargetAmount - currentAmount; // Số tiền còn lại cần tiết kiệm
                var progress = savingGoalDomain.TargetAmount > 0 // Tiến độ tiết kiệm (%)
                    ? Math.Round((double)(currentAmount / savingGoalDomain.TargetAmount) * 100, 2) : 0;

                var savingGoalResponse = new SavingGoalResponse
                {
                    IdSaving = savingGoalDomain.IdSaving,
                    SavingName = savingGoalDomain.SavingName,
                    TargetAmount = savingGoalDomain.TargetAmount,
                    CurrentAmount = currentAmount,
                    RemainingAmount = remaining < 0 ? 0 : remaining,
                    SavingProgress = progress,
                    TargetDate = savingGoalDomain.TargetDate,
                    Status = savingGoalDomain.Status,
                    Description = savingGoalDomain.Description,
                    CreateAt = savingGoalDomain.CreateAt,
                    UrlImage = imageUrl,
                    SavingDetail = savingGoalDomain.SavingDetails.Select(d => new SavingDetailResponse
                    {
                        IdDetail = d.IdDetail,
                        Amount = d.Amount,
                        CreatedAt = d.CreatedAt
                    }).ToList()
                };
                return ApiResponse<SavingGoalResponse>.SuccessResponse("Lấy thông tin mục tiêu tiết kiệm thành công!", 200, savingGoalResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<SavingGoalResponse>.FailResponse(ex.Message, 500);
            }
        }
    }
}
