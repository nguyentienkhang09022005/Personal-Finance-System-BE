using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;

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

        //public async Task<ApiResponse<string>> CreateInvestmentDetailHandleAsync(InvestmentDetailRequest investmentDetailRequest)
        //{
        //    try
        //    {
        //        bool assetExist = await _investmentAssetRepository.CheckExistInvestmentAssetAsync(investmentDetailRequest.IdAsset);
        //        if (!assetExist)
        //            return ApiResponse<string>.FailResponse("Không tìm thấy tài sản!", 404);

        //        if (investmentDetailRequest.Type == "Mua")
        //        {

        //        }
        //        else
        //        {

        //        }
        //        var investmentAssetDomain = _mapper.Map<InvestmentAssetDomain>(investmentAssetRequest);
        //        await _investmentAssetRepository.AddInvestmentAssetAsync(investmentAssetDomain);

        //        return ApiResponse<string>.SuccessResponse("Tạo tài sản cho quỹ thành công!", 200, string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<string>.FailResponse("Lỗi hệ thống: " + ex.Message, 500);
        //    }
        //}
    }
}
