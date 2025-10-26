using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.SavingGoals
{
    public class SavingDetailHandler
    {
        private readonly ISavingDetailRepository _savingDetailRepository;
        private readonly ISavingGoalRepository _savingGoalRepository;
        private readonly IMapper _mapper;

        public SavingDetailHandler(ISavingDetailRepository savingDetailRepository,
                                   ISavingGoalRepository savingGoalRepository,
                                   IMapper mapper)
        {
            _savingDetailRepository = savingDetailRepository;
            _savingGoalRepository = savingGoalRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateSavingDetailAsync(SavingDetailRequest savingDetailRequest)
        {
            try
            {
                var savingDetailDomain = _mapper.Map<SavingDetailDomain>(savingDetailRequest);
                await _savingDetailRepository.AddSavingDetailAsync(savingDetailDomain);
                return ApiResponse<string>.SuccessResponse("Tạo lịch sử tiết kiệm thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteSavingDetailAsync(Guid idSavingDetail)
        {
            try
            {
                await _savingDetailRepository.DeleteSavingDetailAsync(idSavingDetail);
                return ApiResponse<string>.SuccessResponse("Xóa lịch sử tiết kiệm thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<SavingDetailResponse>>> GetListSavingDetailsAsync(Guid idSavingGoal)
        {
            try
            {
                var checkSavingGoalExist = await _savingGoalRepository.ExistSavingGoalDomain(idSavingGoal);
                if (!checkSavingGoalExist)
                {
                    return ApiResponse<List<SavingDetailResponse>>.FailResponse("Không tìm thấy mục tiêu tiết kiệm!", 404);
                }

                var savingDetailDomains = await _savingDetailRepository.GetListSavingDetailsAsync(idSavingGoal);
                var savingGoalResponses = _mapper.Map<List<SavingDetailResponse>>(savingDetailDomains);
                return ApiResponse<List<SavingDetailResponse>>.SuccessResponse("Lấy danh sách lịch sử mục tiêu thành công!", 
                                                                                200, 
                                                                                savingGoalResponses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SavingDetailResponse>>.FailResponse(ex.Message, 500);
            }
        }
    }
}
