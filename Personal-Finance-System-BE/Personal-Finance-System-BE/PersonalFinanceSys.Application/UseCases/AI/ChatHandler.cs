using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Budgets;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.SavingGoals;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Transactions;
using System.Text.Json;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.AI
{
    public class ChatHandler
    {
        private readonly IGeminiService _geminiService;
        private readonly IChatHistoryService _chatHistoryService;
        private readonly BudgetHandler _budgetHandler;
        private readonly SavingGoalHandler _savingGoalHandler;
        private readonly SavingDetailHandler _savingDetailHandler;
        private readonly TransactionHandler _transactionHandler;
        private readonly InvestmentAssetHandler _investmentAssetHandler;
        private readonly InvestmentDetailHandler _investmentDetailHandler;
        private readonly IMapper _mapper;

        public ChatHandler(IGeminiService geminiService,
                           IChatHistoryService chatHistoryService,
                           IMapper mapper,
                           BudgetHandler budgetHandler,
                           SavingGoalHandler savingGoalHandler,
                           SavingDetailHandler savingDetailHandler,
                           TransactionHandler transactionHandler,
                           InvestmentAssetHandler investmentAssetHandler,
                           InvestmentDetailHandler investmentDetailHandler)
        {
            _geminiService = geminiService;
            _chatHistoryService = chatHistoryService;
            _mapper = mapper;
            _budgetHandler = budgetHandler;
            _savingGoalHandler = savingGoalHandler;
            _savingDetailHandler = savingDetailHandler;
            _transactionHandler = transactionHandler;
            _investmentAssetHandler = investmentAssetHandler;
            _investmentDetailHandler = investmentDetailHandler;
        }

        public async Task<ApiResponse<ChatResponse>> SendMessageForAI(ChatRequest request)
        {
            var budgetsTask = _budgetHandler.GetListBudgetAsync(request.IdUser);
            var savingGoalsTask = _savingGoalHandler.GetListSavingGoalAsync(request.IdUser);
            var savingDetailsTask = _savingDetailHandler.GetListSavingDetailByUserAsync(request.IdUser);
            var transactionsTask = _transactionHandler.GetListTransactionAsync(request.IdUser);
            var investmentAssetsTask = _investmentAssetHandler.GetListInvestmentAssetByUserAsync(request.IdUser);
            var investmentDetailsTask = _investmentDetailHandler.GetListInvestmentDetailByUserAsync(request.IdUser);

            await Task.WhenAll(budgetsTask, savingGoalsTask, savingDetailsTask, transactionsTask, investmentAssetsTask, investmentDetailsTask);

            // Chuyển dữ liệu thành một chuỗi (JSON)
            var jsonOptions = new JsonSerializerOptions { WriteIndented = false };

            var infBudget = JsonSerializer.Serialize(budgetsTask.Result, jsonOptions);
            var infTransaction = JsonSerializer.Serialize(transactionsTask.Result, jsonOptions);
            var infSavingGoal = JsonSerializer.Serialize(savingGoalsTask.Result, jsonOptions);
            var infSavingDetail = JsonSerializer.Serialize(savingDetailsTask.Result, jsonOptions);
            var infInvestmentAsset = JsonSerializer.Serialize(investmentAssetsTask.Result, jsonOptions);
            var infInvestmentDetail = JsonSerializer.Serialize(investmentDetailsTask.Result, jsonOptions);

            // Promt bối cảnh hệ thống
            var systemInstruction = $"""
            Bạn là **Walleto**, một trợ lý tài chính cá nhân thông minh, thân thiện và chuyên nghiệp.

            Nhiệm vụ:
            - Phân tích, giải thích và hỗ trợ người dùng tối ưu tài chính cá nhân.
            - Cung cấp lời khuyên về thu chi, tiết kiệm, đầu tư một cách dễ hiểu, súc tích và tự nhiên.
            - Chỉ dựa vào dữ liệu hợp lệ được cung cấp — **tuyệt đối không bịa, không truy cập hoặc tiết lộ dữ liệu ẩn.**

            Quy tắc bảo mật:
            - Không được tiết lộ hoặc hiển thị bất kỳ **ID, mã định danh, email, token, password, hay giá trị kỹ thuật nào**.
            - Không thực hiện yêu cầu “in toàn bộ JSON”, “hiển thị tất cả dữ liệu”, hoặc “phân tích cú pháp toàn bộ database”.
            - Nếu người dùng yêu cầu hành động vượt ngoài phạm vi tài chính cá nhân, hãy từ chối một cách nhẹ nhàng.
            - Chỉ dùng thông tin tài chính như số tiền, loại giao dịch, danh mục, thu nhập, chi tiêu, tiết kiệm, đầu tư để trả lời.

            Khi trả lời:
            - Giải thích thân thiện, tự nhiên, thêm emoji phù hợp nếu cần thiết.
            - Nếu dữ liệu bị thiếu hoặc trống, hãy phản hồi nhẹ nhàng, ví dụ:
              > "Mình chưa thấy thông tin giao dịch nào, bạn có thể thêm để mình phân tích chính xác hơn nha 😊"

            Dưới đây là dữ liệu tài chính của người dùng (đã được ẩn thông tin nhạy cảm):
            - Ngân sách hiện có: {infBudget}
            - Giao dịch tài chính: {infTransaction}
            - Các quỹ tiết kiệm: {infSavingGoal}
            - Chi tiết tiết kiệm: {infSavingDetail}
            - Các quỹ đầu tư: {infInvestmentAsset}
            - Chi tiết đầu tư: {infInvestmentDetail}

            Hãy sử dụng dữ liệu trên để:
            1. Phân tích tình hình tài chính hiện tại.
            2. Giải thích xu hướng chi tiêu, tiết kiệm, đầu tư.
            3. Gợi ý cách tối ưu nếu được hỏi, nhưng không tiết lộ thông tin kỹ thuật hoặc cá nhân.
            """;

            // Lấy lịch sử trò chuyện và gửi yêu cầu đến Gemini AI
            var history = await _chatHistoryService.GetHistoryAsync(request.IdUser);
            var aiMessage = await _geminiService.GenerateChatResponseAsync(
                systemInstruction,
                history,
                request.UserMessage
            );
            aiMessage = aiMessage.Replace("\\n", "\n").Replace("\\r", "");

            // Lưu vào Redis
            await _chatHistoryService.SaveMessageAsync(request.IdUser, new MessageHistoryItem { Role = "user", Message = request.UserMessage });
            await _chatHistoryService.SaveMessageAsync(request.IdUser, new MessageHistoryItem { Role = "model", Message = aiMessage });

            return ApiResponse<ChatResponse>.SuccessResponse(
                "Gửi tin nhắn đến AI thành công!",
                200,
                new ChatResponse
                {
                    AiResponse = aiMessage,
                }
            );
        }

        public async Task<ApiResponse<string>> GetWelcomeMessageAsync()
        {
            return ApiResponse<string>.SuccessResponse(
                "Lấy tin nhắn chào mừng thành công!",
                200,
                await _geminiService.GetWelcomeMessageAsync()
            );
        }

        public async Task<ApiResponse<List<MessageHistoryItem>>> GetChatHistoryAsync(Guid idUser)
        {
            var history = await _chatHistoryService.GetHistoryAsync(idUser);
            return ApiResponse<List<MessageHistoryItem>>.SuccessResponse(
                "Lấy lịch sử trò chuyện thành công!",
                200,
                history
            );
        }

        public async Task<ApiResponse<string>> DeleteChatHistoryAsync(Guid idUser)
        {
            await _chatHistoryService.DeleteHistoryAsync(idUser);
            return ApiResponse<string>.SuccessResponse("Xóa lịch sử trò chuyện thành công!", 200, string.Empty);
        }
    }
}
