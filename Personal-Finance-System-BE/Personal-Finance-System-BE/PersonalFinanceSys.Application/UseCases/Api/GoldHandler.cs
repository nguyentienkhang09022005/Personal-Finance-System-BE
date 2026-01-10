using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api
{
    public class GoldHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiGold;
        private readonly string _urlSJC;
        private readonly string _urlDOJI;
        private readonly string _urlPNJ;

        public GoldHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiGold = configuration["GoldSettings:ApiKey"] ?? throw new ArgumentNullException("GoldSettings:ApiKey");
            _urlSJC = configuration["GoldSettings:UrlSJC"] ?? throw new ArgumentNullException("GoldSettings:UrlSJC");
            _urlDOJI = configuration["GoldSettings:UrlDOJI"] ?? throw new ArgumentNullException("GoldSettings:UrlDOJI");
            _urlPNJ = configuration["GoldSettings:UrlPNJ"] ?? throw new ArgumentNullException("GoldSettings:UrlPNJ");
        }

        public async Task<ApiResponse<SjcGoldResponse>> GetSJCGoldPricesAsync()
        {
            try
            {
                var finalUrl = $"{_urlSJC}?api_key={_apiGold}";
                var response = await _httpClient.GetAsync(finalUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<SjcGoldResponse>.FailResponse(
                        $"Lỗi lấy giá vàng SJC: {response.ReasonPhrase}. Chi tiết: {errorContent}",
                        (int)response.StatusCode);
                }

                var apiResult = await response.Content.ReadFromJsonAsync<SjcGoldResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<SjcGoldResponse>.FailResponse("Dữ liệu giá vàng SJC rỗng!", 404);
                }

                return ApiResponse<SjcGoldResponse>.SuccessResponse("Lấy giá vàng SJC thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<SjcGoldResponse>.FailResponse($"Lỗi hệ thống SJC: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<DojiGoldResponse>> GetDOJIGoldPricesAsync()
        {
            try
            {
                var finalUrl = $"{_urlDOJI}?api_key={_apiGold}";
                var response = await _httpClient.GetAsync(finalUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<DojiGoldResponse>.FailResponse(
                        $"Lỗi lấy giá vàng DOJI: {response.ReasonPhrase}. Chi tiết: {errorContent}",
                        (int)response.StatusCode);
                }

                var apiResult = await response.Content.ReadFromJsonAsync<DojiGoldResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<DojiGoldResponse>.FailResponse("Dữ liệu giá vàng DOJI rỗng!", 404);
                }

                return ApiResponse<DojiGoldResponse>.SuccessResponse("Lấy giá vàng DOJI thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<DojiGoldResponse>.FailResponse($"Lỗi hệ thống DOJI: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PnjGoldResponse>> GetPNJGoldPricesAsync()
        {
            try
            {
                var finalUrl = $"{_urlPNJ}?api_key={_apiGold}";
                var response = await _httpClient.GetAsync(finalUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<PnjGoldResponse>.FailResponse(
                        $"Lỗi lấy giá vàng PNJ: {response.ReasonPhrase}. Chi tiết: {errorContent}",
                        (int)response.StatusCode);
                }

                var apiResult = await response.Content.ReadFromJsonAsync<PnjGoldResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<PnjGoldResponse>.FailResponse("Dữ liệu giá vàng PNJ rỗng!", 404);
                }

                return ApiResponse<PnjGoldResponse>.SuccessResponse("Lấy giá vàng PNJ thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<PnjGoldResponse>.FailResponse($"Lỗi hệ thống PNJ: {ex.Message}", 500);
            }
        }
    }
}