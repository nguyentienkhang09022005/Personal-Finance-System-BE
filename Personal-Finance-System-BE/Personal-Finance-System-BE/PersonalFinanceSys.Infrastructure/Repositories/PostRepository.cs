using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IMapper _mapper;

        public PostRepository(PersonFinanceSysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDomain> AddPostAsync(PostDomain postDomain)
        {
            var post = _mapper.Map<Post>(postDomain);
            post.IdPost = Guid.NewGuid();
            post.IsApproved = false;

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return _mapper.Map<PostDomain>(post);
        }

        public async Task DeletePostAsync(Guid idPost)
        {
            var post = await _context.Posts.FindAsync(idPost)
                            ?? throw new NotFoundException("Không tìm bài đăng cần xóa!");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> GetExistPostAsync(Guid idPost)
        {
            var user = await _context.Posts
                .Include(p => p.IdUserNavigation)
                .FirstOrDefaultAsync(p => p.IdPost == idPost);

            return user ?? throw new NotFoundException("Không tìm thấy bài đăng!");
        }

        public async Task<bool> ExistPostAsync(Guid idPost)
        {
            return await _context.Posts
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .AnyAsync(p => p.IdPost == idPost);
        }

        public async Task<List<PostDomain?>> GetListPostNotApprovedAsync()
        {
            var posts = await _context.Posts
                .Where(p => p.IsApproved == false)
                .Include(p => p.IdUserNavigation)
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<PostDomain?>>(posts);
        }

        public async Task<List<PostDomain?>> GetListPostApprovedAsync()
        {
            var posts = await _context.Posts
                .Where(p => p.IsApproved == true)
                .Include(p => p.IdUserNavigation)
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<PostDomain?>>(posts);
        }

        public async Task<List<PostDomain?>> GetListPostByUserIdAsync(Guid idUser)
        {
            var posts = _context.Posts
                .Include(p => p.IdUserNavigation)
                .Where(p => p.IdUser == idUser)
                .AsNoTracking()
                .ToList();
            return _mapper.Map<List<PostDomain>>(posts);
        }

        public async Task<PostDomain?> UpdatePostAsync(PostDomain postDomain, Post post)
        {
            _mapper.Map(postDomain, post);

            await _context.SaveChangesAsync();
            return _mapper.Map<PostDomain>(post);
        }
    }
}
