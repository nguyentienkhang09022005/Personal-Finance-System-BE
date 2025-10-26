using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GenerateChatResponseAsync(string systemInstruction,
                                               List<MessageHistoryItem> history,
                                               string userMessage);

        Task<string> GetWelcomeMessageAsync();
    }
}
