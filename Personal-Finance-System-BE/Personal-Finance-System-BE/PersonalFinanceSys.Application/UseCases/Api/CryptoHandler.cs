using DotNetEnv;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api
{
    public class CryptoHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlListCrypto;
        private readonly string _apiKey;

        public CryptoHandler(HttpClient httpClient)
        {
            Env.Load(); 
            _httpClient = httpClient; 
            _urlListCrypto = Env.GetString("URL__CRYPTO") ?? throw new Exception("Không tìm thấy key trong .env!");
            _apiKey = Env.GetString("API__KEYCRYPTO") ?? throw new Exception("Không tìm thấy key trong .env!");
        }

        public async Task<ApiResponse<List<CryptoResponse>>> GetListCryptoAsync()
        {
            try
            {
                string urlList = $"{_urlListCrypto}{_apiKey}";

                var response = await _httpClient.GetAsync(urlList);
                if (!response.IsSuccessStatusCode)
                    return ApiResponse<List<CryptoResponse>>.FailResponse("Không thể kết nối đến máy chủ lấy danh sách crypto", 
                                                                              (int)response.StatusCode);

                // Chuyển đổi qua Json
                var apiResult = await response.Content.ReadFromJsonAsync<List<CryptoResponse>>();

                if (apiResult == null)
                {
                    return ApiResponse<List<CryptoResponse>>.FailResponse("Phản hồi API không hợp lệ!", 404);
                }

                return ApiResponse<List<CryptoResponse>>.SuccessResponse("Gọi api thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CryptoResponse>>.FailResponse(ex.Message, 500);
            }
        }
    }
}
