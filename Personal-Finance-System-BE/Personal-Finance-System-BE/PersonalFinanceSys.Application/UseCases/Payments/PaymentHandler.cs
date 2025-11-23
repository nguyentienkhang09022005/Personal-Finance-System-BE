using AutoMapper;
using DotNetEnv;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Payments
{
    public class PaymentHandler
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserRepository _userRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;
        private readonly string _AppId;
        private readonly string _Key1;
        private readonly string _Key2;
        private readonly string _urlServer;
        private readonly string _createOrderEndpoint;

        public PaymentHandler(IPaymentRepository paymentRepository, 
                              IHttpClientFactory httpClientFactory,
                              IUserRepository userRepository,
                              IPackageRepository packageRepository,
                              IMapper mapper)
        {
            Env.Load();
            _AppId = Env.GetString("ZALOPAY__APPID")?.Trim() ?? throw new Exception("Không tìm thấy ZALOPAY__APPID trong .env!");
            _Key1 = Env.GetString("ZALOPAY__KEY1")?.Trim() ?? throw new Exception("Không tìm thấy ZALOPAY__KEY1 trong .env!");
            _Key2 = Env.GetString("ZALOPAY__KEY2")?.Trim() ?? throw new Exception("Không tìm thấy ZALOPAY__KEY2 trong .env!");
            _urlServer = Env.GetString("URL__SERVER")?.Trim() ?? throw new Exception("Không tìm thấy URL__SERVER trong .env!");
            _createOrderEndpoint = Env.GetString("ZALOPAY__CREATEORDERENDPOINT")?.Trim() ?? throw new Exception("Không tìm thấy ZALOPAY__CREATEORDERENDPOINT trong .env!");

            _paymentRepository = paymentRepository;
            _httpClientFactory = httpClientFactory;
            _userRepository = userRepository;
            _packageRepository = packageRepository;
            _mapper = mapper;   
        }


        public async Task<ApiResponse<PaymentResponse>> CreatePaymentIntentAsync(PaymentRequest paymentRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(paymentRequest.IdUser);
                if (!userExists)
                    return ApiResponse<PaymentResponse>.FailResponse("Không tìm thấy người dùng!", 404);

                bool packageExists = await _packageRepository.CheckExistPackage(paymentRequest.IdPackage);
                if (!packageExists)
                    return ApiResponse<PaymentResponse>.FailResponse("Không tìm thấy gói!", 404);

                bool existingPaymentSuccess = await _paymentRepository.CheckExistPaymentWithStatusSuccess(paymentRequest.IdUser, paymentRequest.IdPackage);
                if (existingPaymentSuccess)
                    return ApiResponse<PaymentResponse>.FailResponse("Người dùng đã mua gói này trước đó!", 400);

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

                // Chuẩn bị dữ liệu gọi ZaloPay
                var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var appUser = paymentRequest.IdUser.ToString();
                int amount = (int)Math.Round(paymentRequest.Amount);
                var embedDataJson = JsonSerializer.Serialize(new { 
                    redirecturl = "http://localhost:3000/dashboard/profile"
                });
                var itemsJson = JsonSerializer.Serialize(Array.Empty<object>());

                var dataToSign = $"{int.Parse(_AppId)}|{idAppTrans}|{appUser}|{amount}|{appTime}|{embedDataJson}|{itemsJson}";
                string mac = ComputeHmacSha256(dataToSign, _Key1);

                string urlCallBack = $"{_urlServer}/api/payment/callback";
                var orderData = new
                {
                    app_id = int.Parse(_AppId),
                    app_user = appUser,
                    app_trans_id = idAppTrans,
                    app_time = appTime,
                    amount = amount,
                    item = itemsJson,
                    description = "Thanh Toán Gói Nâng Cấp Hệ Thống",
                    embed_data = embedDataJson,
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

                var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);

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

                var payment = await _paymentRepository.GetPaymentByIdAppTransAsync(callbackData.AppTransId);
                if (payment == null) throw new Exception("Không tìm thấy giao dịch!");

                if (payment.Status != ConstantStatusPayment.PaymentSuccess){
                    await _paymentRepository.UpdateStatusPaymentAsync(payment.IdPayment, ConstantStatusPayment.PaymentSuccess);
                }

                return ApiResponse<string>.SuccessResponse("Xử lý callback thành công!", 200, string.Empty);
            } catch (Exception ex)
            {
                Console.WriteLine($"[Callback Error] {ex}");
                return ApiResponse<string>.FailResponse($"Lỗi khi xử lý callback: {ex.Message}", 500);
            } 
        }

        public async Task<ApiResponse<string>> CancelPackageAsync(PaymentCancelRequest paymentCancelRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(paymentCancelRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                bool packageExists = await _packageRepository.CheckExistPackage(paymentCancelRequest.IdPackage);
                if (!packageExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy gói!", 404);

                var payment = await _paymentRepository.GetPaymentByUserIdAndPackageIdAsync(paymentCancelRequest.IdUser, 
                                                                                           paymentCancelRequest.IdPackage);
                if (payment == null) throw new Exception("Không tìm thấy giao dịch!");

                if (payment.Status == ConstantStatusPayment.PaymentSuccess)
                {
                    await _paymentRepository.UpdateStatusPaymentAsync(payment.IdPayment, ConstantStatusPayment.PaymentCanceled);
                }

                return ApiResponse<string>.SuccessResponse("Hủy gói thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
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
