using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly CryptoHandler _cryptoHandler;

        public InvestmentDetailHandler(IInvestmentDetailRepository investmentDetailRepository, 
                                       IInvestmentAssetRepository investmentAssetRepository, 
                                       IUserRepository userRepository,
                                       IMapper mapper,
                                       CryptoHandler cryptoHandler)
        {
            _investmentDetailRepository = investmentDetailRepository;
            _investmentAssetRepository = investmentAssetRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _cryptoHandler = cryptoHandler;
        }

        public async Task<ApiResponse<InvestmentDetailResponse>> GetInfInvestmentAssetHandleAsync(Guid idAsset)
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
                return ApiResponse<InvestmentDetailResponse>.SuccessResponse("Lấy thông tin chi tiết tài sản thành công!", 200, response);
            }
            catch (Exception ex)
            {
                return ApiResponse<InvestmentDetailResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        private decimal CalculateValueTotalAsset(List<InvestmentDetailDomain> details, decimal currentPriceVND)
        {
            decimal remainingQuantity = details.Where(i => i.Type == ConstrantBuyAndSell.TypeBuy).Sum(i => i.Quantity)
                                            - details.Where(i => i.Type == ConstrantBuyAndSell.TypeSell).Sum(i => i.Quantity);
            return remainingQuantity * currentPriceVND;
        }

        private decimal CalculateAverageNetCost(List<InvestmentDetailDomain> details)
        {
            decimal totalBuyQuantity = details.Where(i => i.Type == ConstrantBuyAndSell.TypeBuy).Sum(i => i.Quantity);
            decimal totalSellQuantity = details.Where(i => i.Type == ConstrantBuyAndSell.TypeSell).Sum(i => i.Quantity);
            decimal remainingQuantity = totalBuyQuantity - totalSellQuantity;
            decimal totalNetCostBuy = details.Where(i => i.Type == ConstrantBuyAndSell.TypeBuy).Sum(i => i.Expense + i.Fee);

            if (totalBuyQuantity == 0 || remainingQuantity <= 0)
                return 0;

            decimal averageCostPerCoinBeforeSell = totalNetCostBuy / totalBuyQuantity;
            decimal totalNetCostRemaining = totalNetCostBuy - (totalSellQuantity * averageCostPerCoinBeforeSell);

            return totalNetCostRemaining / remainingQuantity;
        }

        private decimal CalculateTotalProfitAndLoss(List<InvestmentDetailDomain> details, decimal currentPriceVND)
        {
            decimal totalBuyQuantity = details.Where(i => i.Type == ConstrantBuyAndSell.TypeBuy).Sum(i => i.Quantity);
            decimal totalSellQuantity = details.Where(i => i.Type == ConstrantBuyAndSell.TypeSell).Sum(i => i.Quantity);
            decimal remainingQuantity = totalBuyQuantity - totalSellQuantity;
            decimal totalNetCostBuy = details.Where(i => i.Type == ConstrantBuyAndSell.TypeBuy).Sum(i => i.Expense + i.Fee);

            if (totalBuyQuantity == 0 || remainingQuantity <= 0)
                return 0;

            decimal averageCostPerCoinBeforeSell = totalNetCostBuy / totalBuyQuantity;
            decimal totalNetCostRemaining = totalNetCostBuy - (totalSellQuantity * averageCostPerCoinBeforeSell);
            decimal valueTotalAsset = remainingQuantity * currentPriceVND;

            return valueTotalAsset - totalNetCostRemaining;
        }

        public async Task<decimal> InvestmentAssetProfitByUserHandleAsync(Guid idUser)
        {
            try
            {
                var investmentAssets = await _investmentAssetRepository.GetAllAssetsByUserAsync(idUser);
                var investmentDetails = await _investmentDetailRepository.GetAllDetailsByUserAsync(idUser);

                // Gom nhóm detail theo asset
                var detailsByAsset = investmentDetails
                    .Where(d => d.IdAsset.HasValue)
                    .GroupBy(d => d.IdAsset!.Value)
                    .ToDictionary(g => g.Key, g => g.ToList());

                decimal totalProfit = 0;
                foreach (var investmentAsset in investmentAssets)
                {
                    if (!detailsByAsset.TryGetValue(investmentAsset.IdAsset, out var assetDetails))
                        continue;

                    var priceResult = await _cryptoHandler.GetCurrentPriceCryptoAsync(investmentAsset.Id);
                    if (priceResult?.Data == null)
                        continue;

                    decimal currentPriceVND = priceResult.Data.MarketData.CurrentPrice.VND;

                    // Tổng số lượng mua, tổng tiền mua + phí
                    var buyDetails = assetDetails.Where(d => d.Type == ConstrantBuyAndSell.TypeBuy).ToList();
                    decimal totalBuyQuantity = buyDetails.Sum(d => d.Quantity);
                    decimal totalBuyExpense = buyDetails.Sum(d => d.Expense + d.Fee);

                    // Tổng số lượng bán, tổng tiền bán + phí
                    var sellDetails = assetDetails.Where(d => d.Type == ConstrantBuyAndSell.TypeSell).ToList();
                    decimal totalSellQuantity = sellDetails.Sum(d => d.Quantity);

                    // Giá vốn trung bình trên 1 đơn vị
                    decimal averageCostPerUnit = totalBuyQuantity > 0 ? totalBuyExpense / totalBuyQuantity : 0;

                    // Tính lợi/lỗ từ các lệnh bán
                    decimal profitFromSell = 0;
                    foreach (var sell in sellDetails)
                    {
                        decimal sellProfit = (sell.Price - averageCostPerUnit) * sell.Quantity - sell.Fee;
                        profitFromSell += sellProfit;
                    }

                    // Số lượng còn lại
                    decimal remainingQuantity = totalBuyQuantity - totalSellQuantity;

                    // Giá trị hiện tại của phần còn lại trừ phí mua
                    decimal remainingValue = remainingQuantity * currentPriceVND;
                    decimal remainingCost = remainingQuantity * averageCostPerUnit;

                    decimal profitFromHolding = remainingValue - remainingCost;

                    // Tổng lợi nhuận/lỗ của asset
                    totalProfit += profitFromSell + profitFromHolding;
                }

                return totalProfit;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi hệ thống: ", ex); 
            }
        }

        public async Task<List<InvestmentDetailDomain>> GetListInvestmentDetailByUserAsync(Guid idUser)
        {
            return await _investmentDetailRepository.GetAllDetailsByUserAsync(idUser);
        }

        public async Task<ApiResponse<string>> CreateInvestmentDetailHandleAsync(InvestmentDetailRequest investmentDetailRequest)
        {
            try
            {
                // Check tài sản tồn tại
                bool assetExist = await _investmentAssetRepository.CheckExistInvestmentAssetAsync(investmentDetailRequest.IdAsset);
                if (!assetExist)
                    return ApiResponse<string>.FailResponse("Không tìm thấy tài sản!", 404);

                if (string.Equals(investmentDetailRequest.Type, ConstrantBuyAndSell.TypeSell, StringComparison.OrdinalIgnoreCase))
                {
                    decimal quantityToSell = investmentDetailRequest.Quantity;

                    if (quantityToSell <= 0)
                    {
                        return ApiResponse<string>.FailResponse("Số lượng bán phải lớn hơn 0!", 400);
                    }

                    decimal netQuantityAvailable = await _investmentDetailRepository.GetNetQuantityForAssetAsync(investmentDetailRequest.IdAsset);

                    if (netQuantityAvailable < quantityToSell)
                    {
                        return ApiResponse<string>.FailResponse(
                            $"Số lượng bán ({quantityToSell}) vượt quá số lượng hiện có ({netQuantityAvailable}).",
                            400); 
                    }
                }

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

        // Hàm so sánh đầu tư giữa 2 năm
        public async Task<ApiResponse<CompareInvestmentDetailByYearResponse>> CompareInvestmentDetailByYearAsync(CompareInvestmentDetailByYearRequest compareInvestmentDetailByYearRequest)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(compareInvestmentDetailByYearRequest.IdUser);
                if (!checkUserExist){
                    return ApiResponse<CompareInvestmentDetailByYearResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var checkAssetExist = await _investmentAssetRepository.CheckExistInvestmentAssetByIdAsync(compareInvestmentDetailByYearRequest.Id);
                if (!checkAssetExist){
                    return ApiResponse<CompareInvestmentDetailByYearResponse>.FailResponse("Không tìm thấy tài sản!", 404);
                }

                var investmentDetails = await _investmentDetailRepository.GetInvestmentDetailsByUserAndYearsAsync(
                    compareInvestmentDetailByYearRequest.IdUser,
                    new[] { compareInvestmentDetailByYearRequest.Year1, compareInvestmentDetailByYearRequest.Year2 });

                if (!investmentDetails.Any()){
                    return ApiResponse<CompareInvestmentDetailByYearResponse>.FailResponse("Không có giao dịch đầu tư loại tài sản trong 2 năm này!", 404);
                }

                var groupByYear = investmentDetails
                    .GroupBy(t => t.CreateAt?.Year)
                    .ToDictionary(g => g.Key!.Value, g => g.ToList());

                // Lấy giao dịch đầu tư của từng năm
                groupByYear.TryGetValue(compareInvestmentDetailByYearRequest.Year1, out var year1Details);
                groupByYear.TryGetValue(compareInvestmentDetailByYearRequest.Year2, out var year2Details);

                year1Details ??= new List<InvestmentDetailDomain>();
                year2Details ??= new List<InvestmentDetailDomain>();

                var year1Summary = CalculateYearlyInvestmentSummary(year1Details, compareInvestmentDetailByYearRequest.Year1);
                var year2Summary = CalculateYearlyInvestmentSummary(year2Details, compareInvestmentDetailByYearRequest.Year2);

                var compareResponse = new CompareInvestmentDetailByYearResponse
                {
                    Year1Summary = year1Summary,
                    Year2Summary = year2Summary
                };

                return ApiResponse<CompareInvestmentDetailByYearResponse>.SuccessResponse("So sánh giao dịch đầu tư giữa hai năm thành công!", 200, compareResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<CompareInvestmentDetailByYearResponse>.FailResponse(ex.Message, 500);
            }
        }

        // Hàm so sánh giao dịch giữa 2 tháng cùng hoặc khác năm
        public async Task<ApiResponse<CompareInvestmentDetailByMonthResponse>> CompareInvestmentDetailByMonthAsync(CompareInvestmentDetailByMonthRequest compareInvestmentDetailByMonthRequest)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(compareInvestmentDetailByMonthRequest.IdUser);
                if (!checkUserExist){
                    return ApiResponse<CompareInvestmentDetailByMonthResponse>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var checkAssetExist = await _investmentAssetRepository.CheckExistInvestmentAssetByIdAsync(compareInvestmentDetailByMonthRequest.Id);
                if (!checkAssetExist){
                    return ApiResponse<CompareInvestmentDetailByMonthResponse>.FailResponse("Không tìm thấy tài sản!", 404);
                }

                var investmentsDetails = await _investmentDetailRepository.GetInvestmentDetailsByUserAndMonthsAsync(
                    compareInvestmentDetailByMonthRequest.IdUser,
                    new[]
                    {
                        (compareInvestmentDetailByMonthRequest.FirstMonth, compareInvestmentDetailByMonthRequest.FirstYear),
                        (compareInvestmentDetailByMonthRequest.SecondMonth, compareInvestmentDetailByMonthRequest.SecondYear)
                    });

                if (!investmentsDetails.Any()){
                    return ApiResponse<CompareInvestmentDetailByMonthResponse>.FailResponse("Không có giao dịch đầu tư loại tài sản trong 2 tháng này!", 404);
                }

                var groupByMonthYear = investmentsDetails
                    .GroupBy(t => new { t.CreateAt!.Value.Month, t.CreateAt!.Value.Year })
                    .ToDictionary(g => (g.Key!.Month, g.Key!.Year), g => g.ToList());

                // Lấy giao dịch đầu tư của từng tháng và năm
                groupByMonthYear.TryGetValue(
                    (compareInvestmentDetailByMonthRequest.FirstMonth, compareInvestmentDetailByMonthRequest.FirstYear),
                    out var month1List);

                groupByMonthYear.TryGetValue(
                    (compareInvestmentDetailByMonthRequest.SecondMonth, compareInvestmentDetailByMonthRequest.SecondYear),
                    out var month2List);

                month1List ??= new List<InvestmentDetailDomain>();
                month2List ??= new List<InvestmentDetailDomain>();

                // Tính tổng mua và bán của mỗi tháng và năm
                var summaryMonth1 = CalculateMonthlyInvestmentSummary(month1List,
                                                                     compareInvestmentDetailByMonthRequest.FirstMonth,
                                                                     compareInvestmentDetailByMonthRequest.FirstYear);
                var summaryMonth2 = CalculateMonthlyInvestmentSummary(month2List,
                                                                     compareInvestmentDetailByMonthRequest.SecondMonth,
                                                                     compareInvestmentDetailByMonthRequest.SecondYear);

                var compareResponse = new CompareInvestmentDetailByMonthResponse
                {
                    Month1Summary = summaryMonth1,
                    Month2Summary = summaryMonth2
                };

                return ApiResponse<CompareInvestmentDetailByMonthResponse>.SuccessResponse("So sánh giao dịch đầu tư giữa hai tháng thành công!", 200, compareResponse);
            }
            catch (Exception ex){
                return ApiResponse<CompareInvestmentDetailByMonthResponse>.FailResponse(ex.Message, 500);
            }
        }

        private MonthlyInvestmentDetailSummary CalculateMonthlyInvestmentSummary(List<InvestmentDetailDomain> details, 
                                                                                 int month, int year)
        {
            var summary = new MonthlyInvestmentDetailSummary
            {
                Month = month,
                Year = year,
                TotalBuy = details.Where(d => d.Type == ConstrantBuyAndSell.TypeBuy).Sum(d => d.Expense),
                TotalQuantityBuy = (int)details.Where(d => d.Type == ConstrantBuyAndSell.TypeBuy).Sum(d => d.Quantity),
                InvestmentDetailBuyDetails = details
                    .Where(d => d.Type == ConstrantBuyAndSell.TypeBuy)
                    .Select(_mapper.Map<CompareInvestmentDetailResponse>)
                    .ToList(),

                TotalSell = details.Where(d => d.Type == ConstrantBuyAndSell.TypeSell).Sum(d => d.Expense),
                TotalQuantitySell = (int)details.Where(d => d.Type == ConstrantBuyAndSell.TypeSell).Sum(d => d.Quantity),
                InvestmentDetailSellDetails = details
                    .Where(d => d.Type == ConstrantBuyAndSell.TypeSell)
                    .Select(_mapper.Map<CompareInvestmentDetailResponse>)
                    .ToList(),
            };

            summary.SpreadBuyAndSellByMonth = summary.TotalBuy - summary.TotalSell;
            return summary;
        }

        private YearlyInvestmentDetailSummary CalculateYearlyInvestmentSummary(List<InvestmentDetailDomain> details, 
                                                                               int year)
        {
            var summary = new YearlyInvestmentDetailSummary
            {
                Year = year,
                TotalBuy = details.Where(d => d.Type == ConstrantBuyAndSell.TypeBuy).Sum(d => d.Expense),
                TotalQuantityBuy = (int)details.Where(d => d.Type == ConstrantBuyAndSell.TypeBuy).Sum(d => d.Quantity),
                InvestmentDetailBuyDetails = details
                    .Where(d => d.Type == ConstrantBuyAndSell.TypeBuy)
                    .Select(_mapper.Map<CompareInvestmentDetailResponse>)
                    .ToList(),

                TotalSell = details.Where(d => d.Type == ConstrantBuyAndSell.TypeSell).Sum(d => d.Expense),
                TotalQuantitySell = (int)details.Where(d => d.Type == ConstrantBuyAndSell.TypeSell).Sum(d => d.Quantity),
                InvestmentDetailSellDetails = details
                    .Where(d => d.Type == ConstrantBuyAndSell.TypeSell)
                    .Select(_mapper.Map<CompareInvestmentDetailResponse>)
                    .ToList(),
            };

            summary.SpreadBuyAndSellByYear = summary.TotalBuy - summary.TotalSell;
            return summary;
        }
    }
}
