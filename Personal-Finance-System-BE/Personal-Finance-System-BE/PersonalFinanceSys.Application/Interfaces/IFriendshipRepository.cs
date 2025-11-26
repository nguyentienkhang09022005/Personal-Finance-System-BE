using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IFriendshipRepository
    {
        Task AddFriendshipAsync(FriendshipDomain friendshipDomain);

        Task DeleteFriendshipAsync(Guid idFriendship);

        Task AcceptFriendshipAsync(FriendshipDomain friendshipDomain, Friendship friendship);

        Task RejectFriendshipAsync(FriendshipDomain friendshipDomain, Friendship friendship);

        Task<List<FriendshipDomain>> GetListFriendshipOfUserAsync(Guid idUser);

        Task<List<FriendshipDomain>> GetListFriendshipSentWithStatusPendingByUserAsync(Guid idUser);

        Task<List<FriendshipDomain>> GetListFriendshipReceivedWithStatusPendingByUserAsync(Guid idUser);

        Task<bool> ExistFriendship(Guid idFriendship);

        Task<Friendship> GetExistFriendship(Guid idFriendship);

        Task<bool> ExistFriendshipByRefId(Guid idRef);
    }
}
