using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces
{
    public interface IPostRepository
    {
        Task<PostDomain> AddPostAsync(PostDomain postDomain);

        Task<List<PostDomain?>> GetListPostNotApprovedAsync();

        Task<List<PostDomain?>> GetListPostApprovedAsync();

        Task<List<PostDomain?>> GetListPostByUserIdAsync(Guid idUser);

        Task DeletePostAsync(Guid idPost);

        Task<PostDomain?> UpdatePostAsync(PostDomain postDomain, Post post);

        Task<bool> ExistPostAsync(Guid idPost);

        Task<Post> GetExistPostAsync(Guid idPost);
    }
}
