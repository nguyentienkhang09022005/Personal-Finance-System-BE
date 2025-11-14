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
        private readonly CryptoHandler _cryptoHandler;
        private readonly InvestmentDetailHandler _investmentDetailHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestmentAssetHandler> _logger;

        public InvestmentAssetHandler(IInvestmentAssetRepository investmentAssetRepository, 
                                      IMapper mapper,
                                      IInvestmentFundRepository investmentFundRepository,
                                      CryptoHandler cryptoHandler,
                                      InvestmentDetailHandler investmentDetailHandler,
                                      ILogger<InvestmentAssetHandler> logger)
        {
            _investmentAssetRepository = investmentAssetRepository;
            _mapper = mapper;
            _investmentFundRepository = investmentFundRepository;
            _cryptoHandler = cryptoHandler;
            _investmentDetailHandler = investmentDetailHandler;
            _logger = logger;
        }

        public async Task<ApiResponse<List<ListInvestmentAssetResponse>>> ListInvestmentAssetAsync(Guid idUser)
        {
            try
            {
                var listInvestmentAsset = await _investmentAssetRepository.GetListInvestmentAssetByUserAsync(idUser);

                var listCryptoAsync = await _cryptoHandler.GetListCryptoAsync();
                if (listCryptoAsync?.Data == null)
                    return ApiResponse<List<ListInvestmentAssetResponse>>.FailResponse("Không lấy được danh sách crypto!", 500);

                var cryptoDict = listCryptoAsync.Data.ToDictionary(c => c.Id, c => c);

                var listResponse = MapListInvestmentAssetResponse(listInvestmentAsset, cryptoDict);
                
                return ApiResponse<List<ListInvestmentAssetResponse>>.SuccessResponse(
                    "Lấy danh sách tài sản của người dùng thành công!", 
                    200, 
                    listResponse);
            }
            catch (Exception ex){
                return ApiResponse<List<ListInvestmentAssetResponse>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InvestmentAssetResponse>> GetInfInvestmentFundHandleAsync(Guid idFund)
        {
            try
            {
                // Kiểm tra quỹ có tài sản không
                var listInvestmentAsset = await _investmentAssetRepository.GetListInvestmentAssetAsync(idFund);
                
                // Kiểm tra danh sách crypto
                var listCryptoAsync = await _cryptoHandler.GetListCryptoAsync();
                if (listCryptoAsync?.Data == null)
                    return ApiResponse<InvestmentAssetResponse>.FailResponse("Không lấy được danh sách crypto!", 500);

                var cryptoDict = listCryptoAsync.Data.ToDictionary(c => c.Id, c => c);


                // Lấy toàn bộ detail của từng asset
                var assetDetails = await LoadAllAssetDetailsAsync(listInvestmentAsset);

                // Ghi log các tài sản bị thiếu chi tiết
                if (assetDetails.Count < listInvestmentAsset.Count)
                {
                    var missingAssetIds = listInvestmentAsset.Select(a => a.IdAsset).Except(assetDetails.Keys);
                }

                // Tổng tài sản và tổng lãi lỗ
                var totalFinanceAndProfit = CalculateTotalFinanceAndProfit(assetDetails);
                var totalTransaction = CalculateTotalTransactionAmount(assetDetails);

                // Tính trung bình tài sản và phần trăm trong quỹ
                var avgFinanceList = CalculateAverageFinanceAssetInFund(listInvestmentAsset, assetDetails);

                var listResponse = MapListInvestmentAssetResponse(listInvestmentAsset, cryptoDict);

                var response = new InvestmentAssetResponse
                {
                    TotalFinanceCurrent = totalFinanceAndProfit.TotalFinanceCurrent,
                    TotalTransactionAmount = totalTransaction.TotalTransactionAmount,
                    TotalProfitAndLoss = totalFinanceAndProfit.TotalProfitAndLoss,
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
            var detailsDictionary = new Dictionary<Guid, InvestmentDetailResponse>();

            foreach (var asset in listAssets)
            {

                try
                {
                    var detailResponse = await _investmentDetailHandler.GetInfInvestmentAssetHandleAsync(asset.IdAsset);

                    if (detailResponse == null)
                    {
                    }
                    else if (!detailResponse.Success || detailResponse.Data == null)
                    {
                        _logger.LogWarning("ApiResponse cho Asset ID: {AssetId} (Tên: {AssetName}) không thành công hoặc Data bị null. Success: {Success}, Message: {Message}",
                            asset.IdAsset, asset.AssetName, detailResponse.Success, detailResponse.Message);
                    }
                    else
                    {
                        _logger.LogDebug("Tải chi tiết thành công cho Asset ID: {AssetId} (Tên: {AssetName})", asset.IdAsset, asset.AssetName);

                        detailsDictionary.Add(asset.IdAsset, detailResponse.Data);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi nghiêm trọng khi gọi GetInfInvestmentAssetHandleAsync cho Asset ID: {AssetId} (Tên: {AssetName})", asset.IdAsset, asset.AssetName);
                }
            }

            return detailsDictionary;
        }

        // Hàm tính trung bình tài sản của từng asset trong quỹ
        private List<AverageFinanceAssetResponse> CalculateAverageFinanceAssetInFund(List<InvestmentAssetDomain> listAssets,
                                                                                          Dictionary<Guid, InvestmentDetailResponse> assetDetails)
        {
            if (assetDetails.Count == 0)
                return new List<AverageFinanceAssetResponse>();

            var totalFinanceCurrent = assetDetails.Values.Sum(d => d.ValueTotalAsset);
            _logger.LogDebug("CalculateAverageFinanceAssetInFund - Tổng tài sản quỹ: {TotalFinance}", totalFinanceCurrent);

            var result = listAssets.Select(asset =>
            {
                assetDetails.TryGetValue(asset.IdAsset, out var detail);

                if (detail == null)
                {
                    _logger.LogWarning("Không tìm thấy chi tiết trong Dictionary khi tính % cho Asset: {AssetName} (ID: {AssetId}). Sẽ trả về 0%.", asset.AssetName, asset.IdAsset);
                }

                decimal totalAsset = detail?.ValueTotalAsset ?? 0;
                decimal percent = totalFinanceCurrent > 0
                    ? Math.Round((totalAsset / totalFinanceCurrent) * 100, 2)
                    : 0;

                _logger.LogTrace("Tính toán cho {AssetName}: TotalAsset = {TotalAsset}, Percent = {Percent}%", asset.AssetName, totalAsset, percent);

                return new AverageFinanceAssetResponse
                {
                    AssetName = asset.AssetName,
                    AverageFinance = totalAsset,
                    PercentageInPortfolio = percent
                };
            }).ToList();

            return result;
        }

        // Hàm tính tổng tài sản hiện tại và tổng lãi lỗ
        private CalculateTotalFinanceAndProfitResponse CalculateTotalFinanceAndProfit(Dictionary<Guid, InvestmentDetailResponse> assetDetails)
        {
            decimal totalFinanceCurrent = 0;
            decimal totalProfitAndLoss = 0;

            foreach (var detail in assetDetails.Values)
            {
                totalFinanceCurrent += detail.ValueTotalAsset;
                totalProfitAndLoss += detail.TotalProfitAndLoss;
            }

            return new CalculateTotalFinanceAndProfitResponse
            {
                TotalFinanceCurrent = totalFinanceCurrent,
                TotalProfitAndLoss = totalProfitAndLoss
            };
        }

        // Hàm tính tổng số tiền giao dịch
        private CalculateTotalTransactionResponse CalculateTotalTransactionAmount(Dictionary<Guid, InvestmentDetailResponse> assetDetails)
        {
            decimal totalTransaction = 0;

            foreach (var detail in assetDetails.Values)
            {
                var totalExpense = detail.listInvestmentDetailResponses?
                    .Where(d => d.Type == "Mua")
                    .Sum(d => d.Expense) ?? 0;
                totalTransaction += totalExpense;
            }

            return new CalculateTotalTransactionResponse
            {
                TotalTransactionAmount = totalTransaction
            };
        }

        private List<ListInvestmentAssetResponse> MapListInvestmentAssetResponse(List<InvestmentAssetDomain> listAssets,
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

        public async Task<List<InvestmentAssetDomain>> GetListInvestmentAssetByUserAsync(Guid idUser)
        {
            return await _investmentAssetRepository.GetAllAssetsByUserAsync(idUser);
        }
    }
}
