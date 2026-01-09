using DotNetEnv;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api
{
    public class CryptoHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlListCrypto;
        private readonly string _urlInfCrypto;
        private readonly string _apiKey;

        public CryptoHandler(HttpClient httpClient)
        {
            Env.Load();
            _httpClient = httpClient;

            if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "PersonalFinanceSystem/1.0");
            }

            _urlListCrypto = Env.GetString("URL__CRYPTO") ?? throw new Exception("Không tìm thấy key trong .env!");
            _urlInfCrypto = Env.GetString("URL__CRYPTOINF") ?? throw new Exception("Không tìm thấy key trong .env!");
            _apiKey = Env.GetString("API__KEYCRYPTO") ?? throw new Exception("Không tìm thấy key trong .env!");
        }

        public async Task<ApiResponse<List<CryptoResponse>>> GetListCryptoAsync()
        {
            try
            {
                string urlList = _urlListCrypto;

                string separator = urlList.Contains("?") ? "&" : "?";

                string finalUrl = $"{urlList}{separator}x_cg_demo_api_key={_apiKey}";

                var response = await _httpClient.GetAsync(finalUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<List<CryptoResponse>>.FailResponse(
                        $"Lỗi kết nối API Crypto: {response.ReasonPhrase}. Chi tiết: {errorContent}",
                        (int)response.StatusCode);
                }

                var apiResult = await response.Content.ReadFromJsonAsync<List<CryptoResponse>>();

                if (apiResult == null)
                {
                    return ApiResponse<List<CryptoResponse>>.FailResponse("Dữ liệu trả về rỗng!", 404);
                }

                return ApiResponse<List<CryptoResponse>>.SuccessResponse("Lấy danh sách thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CryptoResponse>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<InfCryptoResponse>> GetCurrentPriceCryptoAsync(string idCrypto)
        {
            try
            {
                string urlInf = $"{_urlInfCrypto}{idCrypto}?x_cg_demo_api_key={_apiKey}";

                var response = await _httpClient.GetAsync(urlInf);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse<InfCryptoResponse>.FailResponse(
                        $"Lỗi lấy chi tiết Crypto: {response.ReasonPhrase}. Chi tiết: {errorContent}",
                        (int)response.StatusCode);
                }

                var apiResult = await response.Content.ReadFromJsonAsync<InfCryptoResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<InfCryptoResponse>.FailResponse("Dữ liệu trả về rỗng!", 404);
                }

                return ApiResponse<InfCryptoResponse>.SuccessResponse("Lấy chi tiết thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<InfCryptoResponse>.FailResponse(ex.Message, 500);
            }
        }
    }
}
