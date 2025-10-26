using Microsoft.Extensions.Caching.Distributed;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IDistributedCache _cache;

        public RefreshTokenService(IDistributedCache cache)
        {
            _cache = cache;
        }

        // Delete RefreshToken on Redis
        public async Task deleteRefreshToken(string idUser)
        {
            await _cache.RemoveAsync($"refresh_token:{idUser}");
        }

        // Get RefreshToken on Redis
        public async Task<string?> getRefreshToken(string idUser)
        {
            return await _cache.GetStringAsync($"refresh_token:{idUser}");
        }

        // Add RefreshToken on Redis 
        public async Task saveRefreshToken(string idUser, string refreshToken, TimeSpan duration)
        {
            await _cache.SetStringAsync(
                $"refresh_token:{idUser}",
                refreshToken,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = duration
                });
        }
    }
}
