using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentAssetHandler
    {
        private readonly IInvestmentAssetRepository _investmentAssetRepository;
        private readonly IInvestmentFundRepository _investmentFundRepository;
        private readonly IMapper _mapper;

        public InvestmentAssetHandler(IInvestmentAssetRepository investmentAssetRepository, 
                                      IMapper mapper,
                                      IInvestmentFundRepository investmentFundRepository)
        {
            _investmentAssetRepository = investmentAssetRepository;
            _mapper = mapper;
            _investmentFundRepository = investmentFundRepository;
        }

        public async Task<ApiResponse<string>> CreateInvestmentAssetHandleAsync(InvestmentAssetRequest investmentAssetRequest)
        {
            try
            {
                bool fundExist = await _investmentFundRepository.CheckExistInvestmentFund(investmentAssetRequest.IdFund);
                if (!fundExist)
                    return ApiResponse<string>.FailResponse("Không tìm thấy quỹ!", 404);

                var investmentAssetDomain = _mapper.Map<InvestmentAssetDomain>(investmentAssetRequest);
                await _investmentAssetRepository.AddInvestmentAssetAsync(investmentAssetDomain);

                return ApiResponse<string>.SuccessResponse("Tạo tài sản cho quỹ thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteInvestmentAssetAsync(Guid idAsset)
        {
            try
            {
                await _investmentAssetRepository.DeleteInvestmentAssetAsync(idAsset);
                return ApiResponse<string>.SuccessResponse("Xóa tài sản thành công!", 200, string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }
    }
}
