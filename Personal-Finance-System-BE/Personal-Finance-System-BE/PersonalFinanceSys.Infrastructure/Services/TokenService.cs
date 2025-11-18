using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly PersonFinanceSysDbContext _context;
        public TokenService(IConfiguration config, PersonFinanceSysDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<string> generateAccessToken(UserDomain user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            // Lấy danh sách permission mà user đã mua gói
            var permissions = await _context.Payments
                .Where(p => p.IdUser == user.IdUser && p.Status == ConstantStatusPayment.PaymentSuccess && p.IdPackage != null)
                .Include(p => p.IdPackageNavigation!)
                    .ThenInclude(pkg => pkg.PermissionNames)
                .SelectMany(p => p.IdPackageNavigation!.PermissionNames)
                .Select(per => per.PermissionName)
                .Distinct()
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var accessToken = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        public async Task<string> generateRefreshToken(UserDomain user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var permissions = await _context.Payments
                .Where(p => p.IdUser == user.IdUser && p.Status == ConstantStatusPayment.PaymentSuccess && p.IdPackage != null)
                .Include(p => p.IdPackageNavigation!)
                    .ThenInclude(pkg => pkg.PermissionNames)
                .SelectMany(p => p.IdPackageNavigation!.PermissionNames)
                .Select(per => per.PermissionName)
                .Distinct()
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var refreshToken = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["RefreshTokenExpirationDays"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
    }
}
