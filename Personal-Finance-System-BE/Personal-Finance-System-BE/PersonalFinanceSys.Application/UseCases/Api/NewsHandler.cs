using DotNetEnv;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api
{
    public class NewsHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly string _apiHost;

        public NewsHandler(HttpClient httpClient)
        {
            Env.Load();
            _httpClient = httpClient;
            _baseUrl = Env.GetString("CNBC__BASEURL") ?? throw new Exception("Không tìm thấy CNBC__BASEURL trong .env!");
            _apiKey = Env.GetString("CNBC__APIKEY") ?? throw new Exception("Không tìm thấy CNBC__APIKEY trong .env!");
            _apiHost = Env.GetString("CNBC__APIHOST") ?? throw new Exception("Không tìm thấy CNBC__APIHOST trong .env!");
        }

        public async Task<ApiResponse<CnbcTrendingNewsResponse>> GetTrendingNewsAsync(string tag = "Articles", int count = 30)
        {
            try
            {
                string requestUrl = $"{_baseUrl}/news/v2/list-trending?tag={tag}&count={count}";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUrl),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };

                // Gửi request
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<CnbcTrendingNewsResponse>.FailResponse(
                        "Không thể kết nối đến máy chủ lấy tin tức CNBC",
                        (int)response.StatusCode
                    );
                }

                // Chuyển đổi qua CnbcTrendingNewsResponse
                var apiResult = await response.Content.ReadFromJsonAsync<CnbcTrendingNewsResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<CnbcTrendingNewsResponse>.FailResponse("Phản hồi API không hợp lệ!", 404);
                }

                return ApiResponse<CnbcTrendingNewsResponse>.SuccessResponse("Lấy danh sách tin tức trending thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<CnbcTrendingNewsResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<CnbcSpecialReportsResponse>> GetSpecialReportsAsync(int pageSize = 30, int page = 1)
        {
            try
            {
                string requestUrl = $"{_baseUrl}/news/v2/list-special-reports?pageSize={pageSize}&page={page}";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUrl),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };

                // Gửi request
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<CnbcSpecialReportsResponse>.FailResponse(
                        "Không thể kết nối đến máy chủ lấy tin tức CNBC",
                        (int)response.StatusCode
                    );
                }

                // Chuyển đổi qua CnbcSpecialReportsResponse
                var apiResult = await response.Content.ReadFromJsonAsync<CnbcSpecialReportsResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<CnbcSpecialReportsResponse>.FailResponse("Phản hồi API không hợp lệ!", 404);
                }

                return ApiResponse<CnbcSpecialReportsResponse>.SuccessResponse("Lấy danh báo cáo đặc biệt thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<CnbcSpecialReportsResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<CnbcNewsResponse>> GetNewsAsync(int count = 30)
        {
            // Danh sách 3 thị trường (Asia, Europe, US)
            var defaultFranchiseIds = new List<string> { "10000527", "10000528", "100003242" };
            try
            {
                string franchiseParams = string.Join("&", defaultFranchiseIds.Select(id => $"franchiseId={id}"));
                string requestUrl = $"{_baseUrl}/news/v2/list?{franchiseParams}&count={count}";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUrl),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };

                // Gửi request
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<CnbcNewsResponse>.FailResponse(
                        "Không thể kết nối đến máy chủ lấy tin tức CNBC",
                        (int)response.StatusCode
                    );
                }

                // Chuyển đổi qua CnbcSpecialReportsResponse
                var apiResult = await response.Content.ReadFromJsonAsync<CnbcNewsResponse>();

                if (apiResult == null)
                {
                    return ApiResponse<CnbcNewsResponse>.FailResponse("Phản hồi API không hợp lệ!", 404);
                }

                return ApiResponse<CnbcNewsResponse>.SuccessResponse("Lấy danh tin tức thành công!", 200, apiResult);
            }
            catch (Exception ex)
            {
                return ApiResponse<CnbcNewsResponse>.FailResponse(ex.Message, 500);
            }
        }
    }
}
