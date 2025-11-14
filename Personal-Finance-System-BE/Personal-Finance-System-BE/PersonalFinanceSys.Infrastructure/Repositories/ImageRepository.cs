using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using SendGrid.Helpers.Errors.Model;
using System.Linq;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly PersonFinanceSysDbContext _context;
        private readonly IDbContextFactory<PersonFinanceSysDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public ImageRepository(PersonFinanceSysDbContext context,
                               IDbContextFactory<PersonFinanceSysDbContext> contextFactory, 
                               IMapper mapper)
        {
            _context = context;
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task AddImageAsync(ImageDomain imageDomain)
        {
            var image = _mapper.Map<Image>(imageDomain);
            image.IdImage = Guid.NewGuid();

            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetImageUrlByIdRefAsync(Guid idRef, string refType)
        {
            await using var context = _contextFactory.CreateDbContext();
            var image = await context.Images
                .AsNoTracking()
                .Where(img => img.IdRef == idRef && img.RefType == refType)
                .OrderBy(img => img.CreateAt)
                .Select(img => img.Url)
                .OrderBy(img => img)
                .LastOrDefaultAsync();

            return image;
        }

        public async Task<List<string>> GetListImageUrlByIdRefAsync(Guid idRef, string refType)
        {
            var images = await _context.Images
                .AsNoTracking()
                .Where(img => img.IdRef == idRef && img.RefType == refType)
                .Select(img => img.Url)
                .ToListAsync();
            return images;
        }

        public async Task<Dictionary<Guid, string?>> GetImagesByListRefAsync(IEnumerable<Guid> idRefs, string refType)
        {
            if (idRefs == null || !idRefs.Any())
                return new Dictionary<Guid, string?>();

            var idRefList = idRefs.ToList();

            await using var context = _contextFactory.CreateDbContext();

            var result = await context.Images
                .AsNoTracking()
                .Where(img => idRefList.Contains(img.IdRef) && img.RefType == refType)
                .GroupBy(img => img.IdRef)
                .Select(i => new
                {
                    IdRef = i.Key,
                    Url = i.Select(x => x.Url).FirstOrDefault()
                })
                .ToListAsync();

            return result.ToDictionary(x => x.IdRef, x => x.Url);
        }

        public async Task DeleteImageByIdRefAsync(Guid idRef, string refType)
        {
            var image = await _context.Images
                .FirstOrDefaultAsync(img => img.IdRef == idRef && img.RefType == refType)
                ?? throw new NotFoundException("Không tìm thấy hình ảnh cần xóa!");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}
