using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentAssetHandler
    {
        private readonly IInvestmentAssetRepository _investmentAssetRepository;
        private readonly IInvestmentFundRepository _investmentFundRepository;
        private readonly IImageRepository _imageRepository;
        private readonly CryptoHandler _cryptoHandler;
        private readonly InvestmentDetailHandler _investmentDetailHandler;
        private readonly IMapper _mapper;

        public InvestmentAssetHandler(IInvestmentAssetRepository investmentAssetRepository, 
                                      IMapper mapper,
                                      IInvestmentFundRepository investmentFundRepository,
                                      IImageRepository imageRepository,
                                      CryptoHandler cryptoHandler,
                                      InvestmentDetailHandler investmentDetailHandler)
        {
            _investmentAssetRepository = investmentAssetRepository;
            _mapper = mapper;
            _investmentFundRepository = investmentFundRepository;
            _imageRepository = imageRepository;
            _cryptoHandler = cryptoHandler;
            _investmentDetailHandler = investmentDetailHandler;
        }

        public async Task<ApiResponse<InvestmentAssetResponse>> GetInfInvestmentFundHandleAsync(Guid idFund)
        {
            try
            {
                var listInvestmentAsset = await _investmentAssetRepository.GetListInvestmentAssetAsync(idFund);
                if (listInvestmentAsset == null || !listInvestmentAsset.Any())
                    return ApiResponse<InvestmentAssetResponse>.FailResponse("Không có tài sản nào trong quỹ!", 404);

                var listCryptoAsync = await _cryptoHandler.GetListCryptoAsync();
                if (listCryptoAsync?.Data == null)
                    return ApiResponse<InvestmentAssetResponse>.FailResponse("Không lấy được danh sách crypto!", 500);

                var cryptoDict = listCryptoAsync.Data.ToDictionary(c => c.Id, c => c);
                

                // Lấy toàn bộ detail của từng asset
                var assetDetails = await LoadAllAssetDetailsAsync(listInvestmentAsset);

                // Tổng tài sản và tổng lãi lỗ
                var totalFinanceAndProfitTask = CalculateTotalFinanceAndProfitAsync(assetDetails);
                var totalTransactionAmountTask = CalculateTotalTransactionAmountAsync(assetDetails);

                // Tính trung bình tài sản và phần trăm trong quỹ
                var avgFinanceListTask = CalculateAverageFinanceAssetInFundAsync(listInvestmentAsset, assetDetails);

                await Task.WhenAll(totalFinanceAndProfitTask, totalTransactionAmountTask, avgFinanceListTask);

                var totalFinanceAndProfit = totalFinanceAndProfitTask.Result;
                var totalTransaction = totalTransactionAmountTask.Result;
                var avgFinanceList = avgFinanceListTask.Result;


                var listResponse = MapListInvestmentAssetResponse(listInvestmentAsset, cryptoDict);

                var response = new InvestmentAssetResponse
                {
                    TotalFinanceCurrent = totalFinanceAndProfit.TotalFinanceCurrent,
                    TotalTransactionAmount = totalTransaction.TotalTransactionAmount,
                    TotalProfitAndLoss = totalFinanceAndProfit.TotalProfitAndLoss,
                    PortfolioChange24h = 0,
                    listInvestmentAssetResponse = listResponse,
                    AverageFinanceAssets = avgFinanceList
                };
                return ApiResponse<InvestmentAssetResponse>.SuccessResponse("Lấy thông tin chi tiết quỹ thành công!", 200, response);
            }
            catch (Exception ex)
            {
                return ApiResponse<InvestmentAssetResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        // Hàm lấy toàn bộ detail của từng asset
        private async Task<Dictionary<Guid, InvestmentDetailResponse>> LoadAllAssetDetailsAsync(List<InvestmentAssetDomain> listAssets)
        {
            var tasks = listAssets.Select(async asset =>
            {
                var detailResponse = await _investmentDetailHandler.GetInfInvestmentAssetHandleAsync(asset.IdAsset);
                return new { asset.IdAsset, Detail = detailResponse?.Data };
            });

            var results = await Task.WhenAll(tasks);

            return results
                .Where(x => x.Detail != null)
                .ToDictionary(x => x.IdAsset, x => x.Detail!);
        }

        // Hàm tính trung bình tài sản của từng asset trong quỹ
        private async Task<List<AverageFinanceAssetResponse>> CalculateAverageFinanceAssetInFundAsync(List<InvestmentAssetDomain> listAssets,
                                                                                                      Dictionary<Guid, InvestmentDetailResponse> assetDetails)
        {
            if (assetDetails.Count == 0)
                return new List<AverageFinanceAssetResponse>();

            var totalFinanceCurrent = assetDetails.Values.Sum(d => d.ValueTotalAsset);

            var result = await Task.Run(() =>
            {
                return listAssets.Select(asset =>
                {
                    assetDetails.TryGetValue(asset.IdAsset, out var detail);
                    decimal totalAsset = detail?.ValueTotalAsset ?? 0;
                    decimal percent = totalFinanceCurrent > 0
                        ? Math.Round((totalAsset / totalFinanceCurrent) * 100, 2)
                        : 0;

                    return new AverageFinanceAssetResponse
                    {
                        AssetName = asset.AssetName,
                        AverageFinance = totalAsset,
                        PercentageInPortfolio = percent
                    };
                }).ToList();
            });

            return result;
        }


        // Hàm tính tổng tài sản hiện tại và tổng lãi lỗ
        private async Task<CalculateTotalFinanceAndProfitResponse> CalculateTotalFinanceAndProfitAsync(Dictionary<Guid, InvestmentDetailResponse> assetDetails)
        {
            decimal totalFinanceCurrent = 0;
            decimal totalProfitAndLoss = 0;

            await Task.Run(() =>
            {
                foreach (var detail in assetDetails.Values)
                {
                    totalFinanceCurrent += detail.ValueTotalAsset;
                    totalProfitAndLoss += detail.TotalProfitAndLoss;
                }
            });

            return new CalculateTotalFinanceAndProfitResponse
            {
                TotalFinanceCurrent = totalFinanceCurrent,
                TotalProfitAndLoss = totalProfitAndLoss
            };
        }

        // Hàm tính tổng số tiền giao dịch
        private async Task<CalculateTotalTransactionResponse> CalculateTotalTransactionAmountAsync(Dictionary<Guid, InvestmentDetailResponse> assetDetails)
        {
            decimal totalTransaction = 0;

            await Task.Run(() =>
            {
                foreach (var detail in assetDetails.Values)
                {
                    var totalExpense = detail.listInvestmentDetailResponses?
                        .Where(d => d.Type == "Mua")
                        .Sum(d => d.Expense) ?? 0;
                    totalTransaction += totalExpense;
                }
            });

            return new CalculateTotalTransactionResponse
            {
                TotalTransactionAmount = totalTransaction
            };
        }

        private List<ListInvestmentAssetResponse> MapListInvestmentAssetResponse(
            List<InvestmentAssetDomain> listAssets,
            Dictionary<string, CryptoResponse> cryptoDict)
        {
            return listAssets.Select(i =>
            {
                cryptoDict.TryGetValue(i.Id, out var matchedCrypto);

                return new ListInvestmentAssetResponse
                {
                    IdAsset = i.IdAsset,
                    Id = i.Id,
                    AssetName = i.AssetName,
                    AssetSymbol = i.AssetSymbol,
                    CurrentPrice = matchedCrypto?.CurrentPrice ?? 0,
                    MarketCap = matchedCrypto?.MarketCap ?? 0,
                    TotalVolume = matchedCrypto?.TotalVolume ?? 0,
                    PriceChangePercentage24h = matchedCrypto?.PriceChangePercentage24h ?? 0,
                    Url = matchedCrypto?.Image
                };
            }).ToList();
        }

        public async Task<ApiResponse<string>> CreateInvestmentAssetHandleAsync(InvestmentAssetRequest investmentAssetRequest)
        {
            try
            {
                bool fundExist = await _investmentFundRepository.CheckExistInvestmentFund(investmentAssetRequest.IdFund);
                if (!fundExist)
                    return ApiResponse<string>.FailResponse("Không tìm thấy quỹ!", 404);

                var investmentAssetDomain = _mapper.Map<InvestmentAssetDomain>(investmentAssetRequest);
                var savedInvestmentAsset = await _investmentAssetRepository.AddInvestmentAssetAsync(investmentAssetDomain);

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
