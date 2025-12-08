using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class AuthenHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvalidatedTokenRepository _invalidatedTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<OtpHandler> _logger;



        public AuthenHandler(IUserRepository userRepository, 
                            ITokenService tokenService,
                            IMapper mapper,
                            IConfiguration config,
                            IHttpContextAccessor httpContextAccessor,
                            IInvalidatedTokenRepository invalidatedTokenRepository,
                            IRefreshTokenService refreshTokenService,
                            ILogger<OtpHandler> logger)
        {
            _userRepository = userRepository;
            _invalidatedTokenRepository = invalidatedTokenRepository;
            _refreshTokenService = refreshTokenService;
            _tokenService = tokenService;
            _mapper = mapper;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        // Login Handle
        public async Task<ApiResponse<AuthenticationResponse>> LoginHandleAsync(AuthenticationRequest authenticationRequest)
        {
            var userDomain = await _userRepository.GetUserByEmailAsync(authenticationRequest.Email);
            if (userDomain == null){
                return ApiResponse<AuthenticationResponse>.FailResponse("Email hoặc mật khẩu không đúng!", 401);
            }

            if (userDomain.IsActive == false){
                return ApiResponse<AuthenticationResponse>.FailResponse("Tài khoản này đã ngừng hoạt động!", 401);
            }

            if (!BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, userDomain.Password)){
                return ApiResponse<AuthenticationResponse>.FailResponse("Email hoặc mật khẩu không đúng!", 401);
            }

            var accessToken = await _tokenService.generateAccessToken(userDomain);
            var refreshToken = await _tokenService.generateRefreshToken(userDomain);

            // Set Refresh Token on Redis
            await _refreshTokenService.saveRefreshToken(userDomain.IdUser.ToString(),
                                                        refreshToken,
                                                        TimeSpan.FromDays(Convert.ToDouble(_config["JwtSettings:RefreshTokenExpirationDays"] ?? "1"))
            );

            // Set cookie HttpOnly
            var response = _httpContextAccessor.HttpContext!.Response;
            response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                MaxAge = TimeSpan.FromDays(Convert.ToDouble(_config["JwtSettings:RefreshTokenExpirationDays"] ?? "1"))
            });

            var userReponse = _mapper.Map<UserResponse>(userDomain);
            var authenticationResponse = new AuthenticationResponse
            {
                Token = accessToken,
                InfUser = userReponse,
            };

            return ApiResponse<AuthenticationResponse>.SuccessResponse("Đăng nhập thành công!", 200, authenticationResponse);
        }

        // Logout Handle
        public async Task<ApiResponse<string>> LogoutHandleAsync()
        {
            string refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"] ?? string.Empty;
            if (string.IsNullOrEmpty(refreshToken))
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy refresh token!", 400);
            }
            var jwtToken = await VerifyToken(refreshToken);

            // Xóa refresh token trên redis
            var idUser = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<string>.FailResponse("Refresh token không hợp lệ!", 400);
            }
            await _refreshTokenService.deleteRefreshToken(idUser);

            // Thêm jti và expiryTime vào danh sách token bị thu hồi
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var expUnix = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            var expiryTime = DateTime.SpecifyKind(
                DateTimeOffset.FromUnixTimeSeconds(long.Parse(expUnix)).DateTime,
                DateTimeKind.Unspecified
            );

            var invalidatedTokenDomain = new InvalidatedTokenDomain(
                idToken: Guid.Parse(jti), 
                expiryTime: expiryTime
                );
            
            await _invalidatedTokenRepository.AddInvalidatedTokenAsync(invalidatedTokenDomain);

            // Xóa cookie refresh token
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");

            return ApiResponse<string>.SuccessResponse("Đăng xuất thành công!", 200, string.Empty);
        }

        // Refresh Token Handle
        public async Task<ApiResponse<AuthenticationResponse>> RefreshTokenHandleAsync()
        {
            string refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies["refreshToken"] ?? string.Empty;
            if (string.IsNullOrEmpty(refreshToken))
            {
                return ApiResponse<AuthenticationResponse>.FailResponse("Không tìm thấy refresh token!", 400);
            }

            var jwtToken = await VerifyToken(refreshToken);
            var idUser = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<AuthenticationResponse>.FailResponse("Refresh token không hợp lệ!", 400);
            }

            // Kiểm tra refresh token có tồn tại trên redis không
            var existRefreshToken = await _refreshTokenService.getRefreshToken(idUser);
            if (existRefreshToken == null || existRefreshToken != refreshToken)
            {
                return ApiResponse<AuthenticationResponse>.FailResponse("Refresh token không hợp lệ!", 400);
            }

            var userDomain = await _userRepository.GetUserByIdAsync(Guid.Parse(idUser));
            if (userDomain == null)
            {
                return ApiResponse<AuthenticationResponse>.FailResponse("Người dùng không tồn tại!", 404);
            }

            var newAccessToken = await _tokenService.generateAccessToken(userDomain);

            var userReponse = _mapper.Map<UserResponse>(userDomain);
            var authenticationResponse = new AuthenticationResponse
            {
                Token = newAccessToken,
                InfUser = userReponse,
            };
            return ApiResponse<AuthenticationResponse>.SuccessResponse("Refresh token thành công!", 200, authenticationResponse);
        }

        // Introspect Token Handle
        public async Task<ApiResponse<IntrospectResponse>> IntrospectTokenHandleAsync(IntrospectRequest introspectRequest)
        {
            Guid idUser = Guid.Empty;
            bool isValid = true;
            try
            {
                var jwtToken = await VerifyToken(introspectRequest.Token);
                string idUserStr = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (!string.IsNullOrEmpty(idUserStr))
                {
                    idUser = Guid.Parse(idUserStr);
                }

            } catch (ArgumentException ex)
            {
                isValid = false;
            }
            var introspectResponse = new IntrospectResponse
            {
                Valid = isValid,
                IdUser = idUser
            };
            return ApiResponse<IntrospectResponse>.SuccessResponse("Introspect token thành công!", 200, introspectResponse);
        }

        // Verify Token Handle
        public async Task<JwtSecurityToken> VerifyToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var tokenHandle = new JwtSecurityTokenHandler();

            try
            {
                tokenHandle.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidIssuer = jwtSettings["Issuer"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (!string.IsNullOrEmpty(jti))
                {
                    bool isInvalidated = await _invalidatedTokenRepository.ExistByIdAsync(Guid.Parse(jti));
                    if (isInvalidated)
                    {
                        throw new ArgumentException("Token đã bị thu hồi!");
                    }
                }
                return jwtToken;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new ArgumentException("Token đã hết hạn!");
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                throw new ArgumentException("chữ ký Token không hợp lệ!");
            }
            catch (Exception)
            {
                throw new ArgumentException("Token không hợp lệ!");
            }
        }
    }
}
