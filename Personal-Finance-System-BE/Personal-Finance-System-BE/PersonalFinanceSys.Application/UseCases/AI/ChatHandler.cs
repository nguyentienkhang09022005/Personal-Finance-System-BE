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

                Nhiệm vụ của bạn:
                - Hỗ trợ người dùng phân tích, theo dõi và tối ưu hóa tài chính cá nhân.
                - Giải thích rõ ràng, dễ hiểu về thu chi, đầu tư, tiết kiệm.
                - Cung cấp lời khuyên dựa trên dữ liệu thực tế mà người dùng cung cấp.
                - Khi trả lời, **không bịa dữ liệu mới** — chỉ dựa trên thông tin có thật trong dữ liệu JSON của người dùng.
                - Trả lời bằng giọng điệu tự nhiên, ngắn gọn, có thể thêm emoji phù hợp nếu cần thiết.

                Dưới đây là dữ liệu tài chính cá nhân của người dùng (dưới dạng JSON):
                - Ngân sách hiện có: {infBudget}
                - Giao dịch tài chính: {infTransaction}
                - Các quỹ tiết kiệm: {infSavingGoal}
                - Chi tiết các khoản tiết kiệm: {infSavingDetail}
                - Các quỹ đầu tư: {infInvestmentAsset}
                - Chi tiết đầu tư (mua bán cổ phiếu, crypto,...): {infInvestmentDetail}

                Hãy sử dụng toàn bộ thông tin trên để:
                1. Giải đáp các thắc mắc của người dùng về tình hình tài chính.
                2. Phân tích chi tiêu, lợi nhuận, hiệu quả tiết kiệm và đầu tư.
                3. Gợi ý kế hoạch tài chính hoặc tối ưu danh mục đầu tư nếu được hỏi.

                Nếu dữ liệu nào bị thiếu hoặc trống, bạn nên phản hồi nhẹ nhàng, ví dụ:
                > "Hiện mình chưa thấy dữ liệu giao dịch nào, bạn có thể thêm để mình phân tích chính xác hơn nha 😊"
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
