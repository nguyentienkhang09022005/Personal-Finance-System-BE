using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentDetailHandler
    {
        private readonly IInvestmentDetailRepository _investmentDetailRepository;
        private readonly IInvestmentAssetRepository _investmentAssetRepository;
        private readonly IMapper _mapper;
        private readonly CryptoHandler _cryptoHandler;

        public InvestmentDetailHandler(IInvestmentDetailRepository investmentDetailRepository, 
                                       IInvestmentAssetRepository investmentAssetRepository, 
                                       IMapper mapper,
                                       CryptoHandler cryptoHandler)
        {
            _investmentDetailRepository = investmentDetailRepository;
            _investmentAssetRepository = investmentAssetRepository;
            _mapper = mapper;
            _cryptoHandler = cryptoHandler;
        }

        public async Task<ApiResponse<InvestmentDetailResponse>> GetInfInvestmentDetailHandleAsync(Guid idAsset)
        {
            try
            {
                var investmentAsset = await _investmentAssetRepository.GetInfInvestmentAssetAsync(idAsset);
                var listInvestmentDetail = await _investmentDetailRepository.GetListInvestmentDetailAsync(idAsset);

                var priceResponse = await _cryptoHandler.GetCurrentPriceCryptoAsync(investmentAsset.Id);

                if (priceResponse == null || priceResponse.Data == null)
                    return ApiResponse<InvestmentDetailResponse>.FailResponse("Không lấy được giá crypto!", 500);

                decimal currentPriceVND = priceResponse.Data.MarketData.CurrentPrice.VND;

                decimal ValueTotalAsset = CalculateValueTotalAsset(listInvestmentDetail, currentPriceVND);
                decimal AverageNetCost = CalculateAverageNetCost(listInvestmentDetail);
                decimal TotalProfitAndLoss = CalculateTotalProfitAndLoss(listInvestmentDetail, currentPriceVND);

                var listResponse = listInvestmentDetail.Select(i =>
                {
                    decimal currentProfit = 0;
                    if (i.Quantity != 0)
                    {
                        currentProfit = (currentPriceVND - (i.Expense / i.Quantity)) * i.Quantity;
                    }

                    return new ListInvestmentDetailResponse
                    {
                        IdDetail = i.IdDetail,
                        Type = i.Type,
                        Price = i.Price,
                        Quantity = i.Quantity,
                        Fee = i.Fee,
                        Expense = i.Expense,
                        CurrentProfit = currentProfit,
                        Profit = currentProfit > 0 ? 1 : (currentProfit < 0 ? -1 : 0),
                        CreateAt = i.CreateAt
                    };
                }).ToList();

                var response = new InvestmentDetailResponse
                {
                    ValueTotalAsset = ValueTotalAsset,
                    AverageNetCost = AverageNetCost,
                    TotalProfitAndLoss = TotalProfitAndLoss,
                    listInvestmentDetailResponses = listResponse
                };
                return ApiResponse<InvestmentDetailResponse>.SuccessResponse("Thành công!", 200, response);
            }
            catch (Exception ex)
            {
                return ApiResponse<InvestmentDetailResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        private decimal CalculateValueTotalAsset(List<InvestmentDetailDomain> details, decimal currentPriceVND)
        {
            decimal remainingQuantity = details.Where(i => i.Type == "Mua").Sum(i => i.Quantity)
                                            - details.Where(i => i.Type == "Bán").Sum(i => i.Quantity);
            return remainingQuantity * currentPriceVND;
        }

        private decimal CalculateAverageNetCost(List<InvestmentDetailDomain> details)
        {
            decimal totalBuyQuantity = details.Where(i => i.Type == "Mua").Sum(i => i.Quantity);
            decimal totalSellQuantity = details.Where(i => i.Type == "Bán").Sum(i => i.Quantity);
            decimal remainingQuantity = totalBuyQuantity - totalSellQuantity;
            decimal totalNetCostBuy = details.Where(i => i.Type == "Mua").Sum(i => i.Expense + i.Fee);

            if (totalBuyQuantity == 0 || remainingQuantity <= 0)
                return 0;

            decimal averageCostPerCoinBeforeSell = totalNetCostBuy / totalBuyQuantity;
            decimal totalNetCostRemaining = totalNetCostBuy - (totalSellQuantity * averageCostPerCoinBeforeSell);

            return totalNetCostRemaining / remainingQuantity;
        }

        private decimal CalculateTotalProfitAndLoss(List<InvestmentDetailDomain> details, decimal currentPriceVND)
        {
            decimal totalBuyQuantity = details.Where(i => i.Type == "Mua").Sum(i => i.Quantity);
            decimal totalSellQuantity = details.Where(i => i.Type == "Bán").Sum(i => i.Quantity);
            decimal remainingQuantity = totalBuyQuantity - totalSellQuantity;
            decimal totalNetCostBuy = details.Where(i => i.Type == "Mua").Sum(i => i.Expense + i.Fee);

            if (totalBuyQuantity == 0 || remainingQuantity <= 0)
                return 0;

            decimal averageCostPerCoinBeforeSell = totalNetCostBuy / totalBuyQuantity;
            decimal totalNetCostRemaining = totalNetCostBuy - (totalSellQuantity * averageCostPerCoinBeforeSell);
            decimal valueTotalAsset = remainingQuantity * currentPriceVND;

            return valueTotalAsset - totalNetCostRemaining;
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
