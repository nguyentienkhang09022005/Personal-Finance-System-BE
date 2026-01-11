using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using System.Text.Json;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Api
{
    public class GoldHandler
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _urlSJC;
        private readonly string _urlDOJI;
        private readonly string _urlPNJ;

        public GoldHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GoldSettings:ApiKey"] ?? throw new ArgumentNullException("GoldSettings:ApiKey");
            _urlSJC = configuration["GoldSettings:UrlSJC"] ?? throw new ArgumentNullException("GoldSettings:UrlSJC");
            _urlDOJI = configuration["GoldSettings:UrlDOJI"] ?? throw new ArgumentNullException("GoldSettings:UrlDOJI");
            _urlPNJ = configuration["GoldSettings:UrlPNJ"] ?? throw new ArgumentNullException("GoldSettings:UrlPNJ");
        }

        public async Task<ApiResponse<GoldResponse>> GetAllGoldPricesAsync()
        {
            try
            {
                var response = new GoldResponse();

                var taskSjc = FetchAndMapSjcAsync();
                var taskDoji = FetchAndMapDojiAsync();
                var taskPnj = FetchAndMapPnjAsync();

                await Task.WhenAll(taskSjc, taskDoji, taskPnj);

                response.SjcGold = await taskSjc;
                response.DojiGold = await taskDoji;
                response.PnjGold = await taskPnj;

                return ApiResponse<GoldResponse>.SuccessResponse("Lấy tổng hợp giá vàng thành công!", 200, response);
            }
            catch (Exception ex)
            {
                return ApiResponse<GoldResponse>.FailResponse($"Lỗi tổng hợp giá vàng: {ex.Message}", 500);
            }
        }

        private async Task<List<GoldItemResponse>> FetchAndMapSjcAsync()
        {
            try
            {
                var root = await GetJsonRootAsync(_urlSJC);
                if (root.ValueKind == JsonValueKind.Undefined) return new List<GoldItemResponse>();

                var resultItem = root.GetProperty("results")[0];
                var timestamp = resultItem.GetProperty("datetime").GetString();
                var updateTime = UnixTimeStampToDateTime(long.Parse(timestamp ?? "0"));
            
                var list = new List<GoldItemResponse>();
                foreach (var kvp in ConstantTypeGold.SjcMapping)
                {
                    list.Add(new GoldItemResponse
                    {
                        Id = "SJC_" + kvp.Key,
                        Name = kvp.Value,
                        Type = kvp.Key,
                        Location = "Toàn quốc",
                        BuyPrice = ParseDecimal(resultItem, $"buy_{kvp.Key}"),
                        SellPrice = ParseDecimal(resultItem, $"sell_{kvp.Key}"),
                        LastUpdated = updateTime
                    });
                }
                return list;
            }
            catch { return new List<GoldItemResponse>(); }
        }

        private async Task<List<GoldItemResponse>> FetchAndMapDojiAsync()
        {
            try
            {
                var root = await GetJsonRootAsync(_urlDOJI);
                if (root.ValueKind == JsonValueKind.Undefined) return new List<GoldItemResponse>();

                var resultItem = root.GetProperty("results")[0];
                var timestamp = resultItem.GetProperty("datetime").GetString();
                var updateTime = UnixTimeStampToDateTime(long.Parse(timestamp ?? "0"));

                var list = new List<GoldItemResponse>();
                foreach (var kvp in ConstantTypeGold.DojiLocationMapping)
                {
                    list.Add(new GoldItemResponse
                    {
                        Id = "DOJI_" + kvp.Key,
                        Name = "Vàng AVPL / SJC (Lẻ)",
                        Type = kvp.Key,
                        Location = kvp.Value,
                        BuyPrice = ParseDecimal(resultItem, $"buy_{kvp.Key}"),
                        SellPrice = ParseDecimal(resultItem, $"sell_{kvp.Key}"),
                        LastUpdated = updateTime
                    });
                }
                return list;
            }
            catch { return new List<GoldItemResponse>(); }
        }

        private async Task<List<GoldItemResponse>> FetchAndMapPnjAsync()
        {
            try
            {
                var root = await GetJsonRootAsync(_urlPNJ);
                if (root.ValueKind == JsonValueKind.Undefined) return new List<GoldItemResponse>();

                var resultItem = root.GetProperty("results")[0];
                var timestamp = resultItem.GetProperty("datetime").GetString();
                var updateTime = UnixTimeStampToDateTime(long.Parse(timestamp ?? "0"));

                var list = new List<GoldItemResponse>();
                foreach (var kvp in ConstantTypeGold.PnjMapping)
                {
                    list.Add(new GoldItemResponse
                    {
                        Id = "PNJ_" + kvp.Key,
                        Name = kvp.Value,
                        Type = kvp.Key,
                        Location = "Toàn quốc",
                        BuyPrice = ParseDecimal(resultItem, $"buy_{kvp.Key}"),
                        SellPrice = ParseDecimal(resultItem, $"sell_{kvp.Key}"),
                        LastUpdated = updateTime
                    });
                }
                return list;
            }
            catch { return new List<GoldItemResponse>(); }
        }
        private async Task<JsonElement> GetJsonRootAsync(string url)
        {
            var finalUrl = $"{url}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(finalUrl);
            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            return doc.RootElement.Clone();
        }

        private decimal ParseDecimal(JsonElement element, string key)
        {
            if (element.TryGetProperty(key, out var prop))
            {
                var valueStr = prop.GetString();
                if (decimal.TryParse(valueStr, out var result))
                {
                    return result;
                }
            }
            return 0;
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}