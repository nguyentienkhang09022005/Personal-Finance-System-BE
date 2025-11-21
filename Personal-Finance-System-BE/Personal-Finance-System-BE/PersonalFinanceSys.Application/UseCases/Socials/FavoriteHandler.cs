using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials
{
    public class FavoriteHandler
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public FavoriteHandler(IFavoriteRepository favoriteRepository, 
                               IUserRepository userRepository,
                               IPostRepository postRepository,
                               IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateFavoriteAsync(FavoriteRequest favoriteRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(favoriteRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                bool postExists = await _postRepository.ExistPostAsync(favoriteRequest.IdPost);
                if (!postExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy bài đăng!", 404);

                var favoriteDomain = _mapper.Map<FavoriteDomain>(favoriteRequest);

                await _favoriteRepository.AddFavoriteAsync(favoriteDomain);

                return ApiResponse<string>.SuccessResponse("Yêu thích bài đăng thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteFavoriteAsync(FavoriteRequest favoriteRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(favoriteRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                bool postExists = await _postRepository.ExistPostAsync(favoriteRequest.IdPost);
                if (!postExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy bài đăng!", 404);

                await _favoriteRepository.DeleteFavoriteAsync(favoriteRequest.IdUser, favoriteRequest.IdPost);

                return ApiResponse<string>.SuccessResponse("Xóa mục yêu thích thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }
    }
}
