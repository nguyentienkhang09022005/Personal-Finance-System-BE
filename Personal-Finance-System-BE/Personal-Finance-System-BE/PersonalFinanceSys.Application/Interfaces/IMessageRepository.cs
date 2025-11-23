using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(MessageDomain messageDomain);

        Task DeleteMessageAsync(Guid idMessage);

        Task<List<MessageDomain>> GetListMessageAsync(Guid idFriendship);

        Task<bool> ExistMessage(Guid idMessage);

        Task<Message> GetExistMessage(Guid idMessage);
    }
}
