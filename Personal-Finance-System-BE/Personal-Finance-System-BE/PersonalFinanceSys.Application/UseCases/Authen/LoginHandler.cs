using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Authen
{
    public class LoginHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LoginHandler(IUserRepository userRepository, 
                            ITokenService tokenService, 
                            IMapper mapper, 
                            IConfiguration config, 
                            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<AuthenticationResponse>> HandleAsync(AuthenticationRequest authenticationRequest)
        {
            var userDomain = await _userRepository.GetUserByEmailAsync(authenticationRequest.Email);
            if (userDomain == null){
                return ApiResponse<AuthenticationResponse>.FailResponse("Email hoặc mật khẩu không đúng!", 401);
            }

            if (!BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, userDomain.Password)){
                return ApiResponse<AuthenticationResponse>.FailResponse("Email hoặc mật khẩu không đúng!", 401);
            }

            var accessToken = _tokenService.generateAccessToken(userDomain);
            var refreshToken = _tokenService.generateRefreshToken(userDomain);

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
    }
}
