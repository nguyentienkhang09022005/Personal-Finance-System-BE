using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentDetailHandler
    {
        private readonly IInvestmentDetailRepository _investmentDetailRepository;
        private readonly IInvestmentAssetRepository _investmentAssetRepository;
        private readonly IMapper _mapper;

        public InvestmentDetailHandler(IInvestmentDetailRepository investmentDetailRepository, 
                                       IInvestmentAssetRepository investmentAssetRepository, 
                                       IMapper mapper)
        {
            _investmentDetailRepository = investmentDetailRepository;
            _investmentAssetRepository = investmentAssetRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateInvestmentDetailHandleAsync(InvestmentDetailRequest investmentDetailRequest)
        {
            try
            {
                bool assetExist = await _investmentAssetRepository.CheckExistInvestmentAssetAsync(investmentDetailRequest.IdAsset);
                if (!assetExist)
                    return ApiResponse<string>.FailResponse("Không tìm thấy tài sản!", 404);

                
                var investmentDetailDomain = _mapper.Map<InvestmentDetailDomain>(investmentDetailRequest);
                await _investmentDetailRepository.AddInvestmentDetailAsync(investmentDetailDomain);

                return ApiResponse<string>.SuccessResponse("Tạo chi tiết mua bán cho tài sản thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteInvestmentDetailAsync(Guid idDetail)
        {
            try
            {
                await _investmentDetailRepository.DeleteInvestmentDetailAsync(idDetail);
                return ApiResponse<string>.SuccessResponse("Xóa chi tiết thành công!", 200, string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }
    }
}
