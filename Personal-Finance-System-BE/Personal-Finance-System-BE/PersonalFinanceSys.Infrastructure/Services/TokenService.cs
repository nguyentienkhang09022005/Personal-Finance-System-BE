using Microsoft.IdentityModel.Tokens;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        public TokenService(IConfiguration config, IRolePermissionRepository rolePermissionRepository)
        {
            _config = config;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<string> generateAccessToken(UserDomain user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var permissions = await _rolePermissionRepository.GetPermissionNamesByRoleAsync(user.RoleName ?? "");

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
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

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var permissions = await _rolePermissionRepository.GetPermissionNamesByRoleAsync(user.RoleName ?? "");

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
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
