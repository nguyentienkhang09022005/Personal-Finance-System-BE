using AutoMapper;
using DotNetEnv;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Payments
{
    public class PaymentHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly string _AppId;
        private readonly string _Key1;
        private readonly string _Key2;
        private readonly string _urlServer;
        private readonly string _createOrderEndpoint;

        public PaymentHandler(IPaymentRepository paymentRepository, 
                              IHttpClientFactory httpClientFactory, 
                              IMapper mapper)
        {
            Env.Load();
            _AppId = Env.GetString("ZALOPAY__APPID") ?? throw new Exception("Không tìm thấy key trong .env!");
            _Key1 = Env.GetString("ZALOPAY__KEY1") ?? throw new Exception("Không tìm thấy key trong .env!");
            _Key2 = Env.GetString("ZALOPAY__KEY2") ?? throw new Exception("Không tìm thấy key trong .env!");
            _urlServer = Env.GetString("URL__SERVER") ?? throw new Exception("Không tìm thấy key trong .env!");
            _createOrderEndpoint = Env.GetString("ZALOPAY__CREATE_ORDER_ENDPOINT") ?? throw new Exception("Không tìm thấy key trong .env!");

            _paymentRepository = paymentRepository;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }


        public async Task<ApiResponse<PaymentResponse>> CreatePaymentIntentAsync(PaymentRequest paymentRequest)
        {
            try
            {
                // Tạo IdAppTrans
                var vnNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
                var prefix = vnNow.ToString("yyMMdd");
                var rand = new Random().Next(0, 1_000_000);
                var idAppTrans = $"{prefix}_{rand:000000}";

                PaymentDomain paymentDomain = _mapper.Map<PaymentDomain>(paymentRequest);
                paymentDomain.IdAppTrans = idAppTrans;
                paymentDomain.Status = ConstantStatusPayment.PaymentPending;
                paymentDomain.Method = "ZaloPay";

                PaymentResponse paymentResponse = await _paymentRepository.CreatePaymentAsync(paymentDomain);

                // 4Chuẩn bị dữ liệu gọi ZaloPay
                var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var embedData = JsonSerializer.Serialize(new { redirecturl = "http://localhost:3000/profile" });
                var items = JsonSerializer.Serialize(Array.Empty<object>());
                var appUser = paymentRequest.IdUser.ToString();

                string dataToSign = $"{_AppId}|{idAppTrans}|{appUser}|{paymentRequest.Amount}|{appTime}|{embedData}|{items}";
                string mac = ComputeHmacSha256(dataToSign, _Key1);

                string urlCallBack = $"{_urlServer}/api/payment/callback";
                var orderData = new
                {
                    app_id = _AppId,
                    app_user = appUser,
                    app_trans_id = idAppTrans,
                    app_time = appTime,
                    amount = paymentRequest.Amount,
                    item = items,
                    description = "Thanh Toán Đơn Gói Nâng Cấp Hệ Thống",
                    embed_data = embedData,
                    mac = mac,
                    bank_code = "zalopayapp",
                    callback_url = urlCallBack
                };

                // Gửi request đến ZaloPay
                var client = _httpClientFactory.CreateClient();
                var jsonBody = JsonSerializer.Serialize(orderData);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_createOrderEndpoint, content);
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);

                var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);

                // Kiểm tra phản hồi
                if (!result.ContainsKey("return_code") || result["return_code"].GetInt32() != 1)
                {
                    await _paymentRepository.UpdateStatusPaymentAsync(paymentResponse.IdPayment, ConstantStatusPayment.PaymentFailed);
                    var message = result.ContainsKey("return_message") ? result["return_message"].GetString() : "Unknown";
                    throw new Exception($"Tạo thanh toán thất bại! {message}");
                }

                // Thành công
                string orderUrl = result["order_url"].GetString() ?? string.Empty;
                paymentResponse.OrderUrl = orderUrl;
                return ApiResponse<PaymentResponse>.SuccessResponse("Tạo thanh toán thành công!", 200, paymentResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaymentResponse>.FailResponse($"Lỗi khi tạo thanh toán: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<string>> ResolveZaloPayCallbackAsync(ZaloPayCallBackRequest zaloPayCallBackRequest)
        {
            try
            {
                // Kiểm tra MAC với Key2
                string mac = ComputeHmacSha256(zaloPayCallBackRequest.Data, _Key2);
                if (!mac.Equals(zaloPayCallBackRequest.Mac, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Chữ ký không hợp lệ!");

                // Parse dữ liệu callback
                var callbackData = JsonSerializer.Deserialize<ZaloPayCallbackData>(zaloPayCallBackRequest.Data);
                if (callbackData == null) throw new Exception("Dữ liệu callback rỗng!");

                // Lấy payment từ DB
                var payment = await _paymentRepository.GetPaymentByIdAppTransAsync(callbackData.AppTransId);
                if (payment == null) throw new Exception("Không tìm thấy giao dịch!");

                // Cập nhật status
                if (payment.Status != ConstantStatusPayment.PaymentSuccess)
                {
                    await _paymentRepository.UpdateStatusPaymentAsync(payment.IdPayment, ConstantStatusPayment.PaymentSuccess);
                }
                return ApiResponse<string>.SuccessResponse("Xử lý callback thành công!", 200, string.Empty);
            } catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse($"Lỗi khi xử lý callback: {ex.Message}", 500);
            } 
        }

        private static string ComputeHmacSha256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
