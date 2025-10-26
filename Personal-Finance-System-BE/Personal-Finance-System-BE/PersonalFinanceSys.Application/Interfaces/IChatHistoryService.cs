using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IChatHistoryService
    {
        Task SaveMessageAsync(Guid idUser, MessageHistoryItem message);

        Task<List<MessageHistoryItem>> GetHistoryAsync(Guid idUser);

        Task DeleteHistoryAsync(Guid idUser);
    }
}
