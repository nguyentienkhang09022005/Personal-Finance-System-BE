using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentFundHandler
    {
        private readonly IInvestmentFundRepository _investmentFundRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInvestmentAssetRepository _investmentAssetRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly CryptoHandler _cryptoHandler;
        private readonly ILogger<InvestmentFundHandler> _logger;

        public InvestmentFundHandler(IInvestmentFundRepository investmentFundRepository,
            IUserRepository userRepository,
                                     IInvestmentAssetRepository investmentAssetRepository,
                                     IImageRepository imageRepository,
                                     IMapper mapper,
                                     
                                     CryptoHandler cryptoHandler,
                                     ILogger<InvestmentFundHandler> logger)
        {
            _investmentFundRepository = investmentFundRepository;
            _userRepository = userRepository;
            _investmentAssetRepository = investmentAssetRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
            _cryptoHandler = cryptoHandler;
            _logger = logger;
        }

        public async Task<ApiResponse<List<InvestmentFundResponse>>> GetListInvestmentFundHandleAsync(Guid idUser)
        {
            bool userExists = await _userRepository.ExistUserAsync(idUser);
            if (!userExists)
                return ApiResponse<List<InvestmentFundResponse>>.FailResponse("Không tìm thấy người dùng!", 404);

            var funds = await _investmentFundRepository.GetListInvesmentFundDomains(idUser);
            if (funds == null || !funds.Any()){
                return ApiResponse<List<InvestmentFundResponse>>.SuccessResponse("Người dùng không có quỹ nào.", 200, new List<InvestmentFundResponse>());
            }

            var fundIds = funds.Select(f => f.IdFund).ToList();

            // Lấy tất cả assets cho tất cả quỹ
            var allAssets = await _investmentAssetRepository.GetAssetsForMultipleFundsAsync(fundIds);

            var listCryptoAsync = await _cryptoHandler.GetListCryptoAsync();
            var cryptoDict = listCryptoAsync?.Data?.ToDictionary(c => c.Id, c => c)
                             ?? new Dictionary<string, CryptoResponse>();

            // Nhóm assets theo IdFund 
            var assetsByFundIdLookup = allAssets.ToLookup(asset => asset.IdFund);

            var finalFundResponses = new List<InvestmentFundResponse>();
            foreach (var fund in funds)
            {
                var fundResponse = _mapper.Map<InvestmentFundResponse>(fund);
                var assetsForThisFund = assetsByFundIdLookup[fund.IdFund];

                fundResponse.listInvestmentAssetResponses = MapListInvestmentAssetResponse(assetsForThisFund, cryptoDict);

                finalFundResponses.Add(fundResponse);
            }

            return ApiResponse<List<InvestmentFundResponse>>.SuccessResponse(
                "Lấy danh sách quỹ của người dùng thành công!",
                200,
                finalFundResponses
            );
        }

        private List<ListInvestmentAssetResponse> MapListInvestmentAssetResponse(IEnumerable<InvestmentAssetDomain> listAssets,
            Dictionary<string, CryptoResponse> cryptoDict)
        {
            if (listAssets == null)
                return new List<ListInvestmentAssetResponse>();

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

        // Hàm lấy danh sách quỹ cá nhân qua idUser
        //public async Task<ApiResponse<List<InvestmentFundResponse>>> GetListInvestmentFundHandleAsync(Guid idUser)
        //{
        //    bool userExists = await _userRepository.ExistUserAsync(idUser);
        //    if (!userExists)
        //        return ApiResponse<List<InvestmentFundResponse>>.FailResponse("Không tìm thấy người dùng!", 404);

        //    var funds = await _investmentFundRepository.GetListInvesmentFundDomains(idUser);

        //    var fundResposne = _mapper.Map<List<InvestmentFundResponse>>(funds);
        //    return ApiResponse<List<InvestmentFundResponse>>.SuccessResponse("Lấy danh sách quỹ của người dùng thành công!", 200, fundResposne);
        //}

        // Hàm tạo quỹ cá nhân
        public async Task<ApiResponse<InvestmentFundResponse>> CreateInvestmentHandleAsync(InvestmentFundCreationRequest investmentFundCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(investmentFundCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<InvestmentFundResponse>.FailResponse("Không tìm thấy người dùng!", 404);

                var investmentFundDomain = _mapper.Map<InvestmentFundDomain>(investmentFundCreationRequest);
                var investmentFundCreated = await _investmentFundRepository.AddInvestmentAsync(investmentFundDomain);

                if (investmentFundCreationRequest.UrlImage != null)
                {
                    var image = new ImageDomain
                    {
                        Url = investmentFundCreationRequest.UrlImage,
                        IdRef = investmentFundCreated.IdFund,
                        RefType = "INVESTMENT_FUND"
                    };
                    await _imageRepository.AddImageAsync(image);
                }

                var investmentFundResponse = _mapper.Map<InvestmentFundResponse>(investmentFundCreated);
                return ApiResponse<InvestmentFundResponse>.SuccessResponse("Tạo quỹ thành công!", 200, investmentFundResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<InvestmentFundResponse>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        // Hàm thay đổi thông tin của quỹ
        public async Task<ApiResponse<InvestmentFundResponse>> UpdateInvestmentFundAsync(Guid idFund ,InvestmentFundUpdateRequest investmentFundUpdateRequest)
        {
            try
            {
                var investmentFundEntity = await _investmentFundRepository.ExistInvestmentFund(idFund);
                if (investmentFundEntity == null){
                    return ApiResponse<InvestmentFundResponse>.FailResponse("Không tìm thấy quỹ!", 404);
                }
                var investmentDomain = _mapper.Map<InvestmentFundDomain>(investmentFundEntity);

                _mapper.Map(investmentFundUpdateRequest, investmentDomain);

                // Cập nhật ảnh nếu có
                if (investmentFundUpdateRequest.UrlImage != null)
                {
                    var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idFund, "INVESTMENT_FUND");
                    if (!string.IsNullOrEmpty(existingImageUrl))
                    {
                        await _imageRepository.DeleteImageByIdRefAsync(idFund, "INVESTMENT_FUND");
                    }

                    var image = new ImageDomain
                    {
                        Url = investmentFundUpdateRequest.UrlImage,
                        IdRef = idFund,
                        RefType = "INVESTMENT_FUND"
                    };

                    await _imageRepository.AddImageAsync(image);
                }
                var investmentFundUpdated = await _investmentFundRepository.UpdateInvestmentFundAsync(investmentDomain, investmentFundEntity);

                var investmentFundResponse = _mapper.Map<InvestmentFundResponse>(investmentFundUpdated);
                return ApiResponse<InvestmentFundResponse>.SuccessResponse("Cập nhật thông tin quỹ thành công", 200, investmentFundResponse);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<InvestmentFundResponse>.FailResponse(ex.Message, 404);
            }
        }

        // Hàm xóa quỹ
        public async Task<ApiResponse<string>> DeleteInvestmentHandleAsync(Guid idFund)
        {
            try
            {
                await _investmentFundRepository.DeleteInvestmentFundAsync(idFund);
                return ApiResponse<string>.SuccessResponse("Xóa quỹ thành công!", 200, string.Empty);
            }
            catch (NotFoundException ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 404);
            }
        }
    }
}
