using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using System.Text.Json;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Posts
{
    public class PostHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public PostHandler(IPostRepository postRepository, 
                           IUserRepository userRepository, 
                           IImageRepository imageRepository,
                           IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<PostResponse>>> GetListPostsByUserIdAsync(Guid idUser)
        {
            try
            {
                var checkUserExist = await _userRepository.ExistUserAsync(idUser);
                if (!checkUserExist){
                    return ApiResponse<List<PostResponse>>.FailResponse("Không tìm thấy người dùng!", 404);
                }

                var posts = await _postRepository.GetListPostByUserIdAsync(idUser);
                if (posts == null || !posts.Any())
                {
                    return ApiResponse<List<PostResponse>>.SuccessResponse(
                        "Chưa có bài đăng nào được tạo!", 
                        200, 
                        new List<PostResponse>());
                }

                var postIds = posts.Select(p => p.IdPost).ToList();
                var imagesDict = await _imageRepository.GetImagesByListRefAsync(postIds, "POSTS");

                var listResponse = new List<PostResponse>();
                foreach (var post in posts)
                {
                    var postResponse = _mapper.Map<PostResponse>(post);

                    // Avatar User
                    var avatarUser = await _imageRepository.GetImageUrlByIdRefAsync(idUser, "USER");
                    if (!string.IsNullOrEmpty(avatarUser)){
                        postResponse.UserOfPostResponse.UrlAvatar = avatarUser;
                    }

                    // Snapshot
                    if (!string.IsNullOrEmpty(post.Snapshot))
                    {
                        try
                        {
                            var snapshot = JsonSerializer.Deserialize<SnapshotResponse>(
                                post.Snapshot,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );
                            postResponse.SnapshotResponse = snapshot != null ? new List<SnapshotResponse> { snapshot } : null;
                        }
                        catch (JsonException){
                            postResponse.SnapshotResponse = null;
                        }
                    }

                    if (imagesDict.TryGetValue(post.IdPost, out var imageUrl)){
                        postResponse.UrlImage = imageUrl;
                    }

                    listResponse.Add(postResponse);
                }


                return ApiResponse<List<PostResponse>>.SuccessResponse("Lấy danh sách gói thành công!", 200, listResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PostResponse>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<PostResponse>>> GetListPostsNotApprovedAsync()
        {
            try
            {
                var posts = await _postRepository.GetListPostNotApprovedAsync();
                if (posts == null || !posts.Any())
                {
                    return ApiResponse<List<PostResponse>>.SuccessResponse(
                        "Chưa có bài đăng nào được tạo!",
                        200,
                        new List<PostResponse>());
                }

                var postIds = posts.Select(p => p.IdPost).ToList();
                var imagesDict = await _imageRepository.GetImagesByListRefAsync(postIds, "POSTS");

                var listResponse = new List<PostResponse>();
                foreach (var post in posts)
                {
                    var postResponse = _mapper.Map<PostResponse>(post);

                    // Avatar User
                    var avatarUser = await _imageRepository.GetImageUrlByIdRefAsync(post.IdUser, "USER");
                    if (!string.IsNullOrEmpty(avatarUser))
                    {
                        postResponse.UserOfPostResponse.UrlAvatar = avatarUser;
                    }

                    // Snapshot
                    if (!string.IsNullOrEmpty(post.Snapshot))
                    {
                        try
                        {
                            var snapshot = JsonSerializer.Deserialize<SnapshotResponse>(
                                post.Snapshot,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );
                            postResponse.SnapshotResponse = snapshot != null ? new List<SnapshotResponse> { snapshot } : null;
                        }
                        catch (JsonException){
                            postResponse.SnapshotResponse = null;
                        }
                    }

                    if (imagesDict.TryGetValue(post.IdPost, out var imageUrl)){
                        postResponse.UrlImage = imageUrl;
                    }

                    listResponse.Add(postResponse);
                }

                return ApiResponse<List<PostResponse>>.SuccessResponse("Lấy danh sách gói thành công!", 200, listResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PostResponse>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<PostResponse>>> GetListPostsApprovedAsync()
        {
            try
            {
                var posts = await _postRepository.GetListPostApprovedAsync();
                if (posts == null || !posts.Any())
                {
                    return ApiResponse<List<PostResponse>>.SuccessResponse(
                        "Chưa có bài đăng nào được tạo!",
                        200,
                        new List<PostResponse>());
                }

                var postIds = posts.Select(p => p.IdPost).ToList();
                var imagesDict = await _imageRepository.GetImagesByListRefAsync(postIds, "POSTS");

                var listResponse = new List<PostResponse>();
                foreach (var post in posts)
                {
                    var postResponse = _mapper.Map<PostResponse>(post);

                    // Avatar User
                    var avatarUser = await _imageRepository.GetImageUrlByIdRefAsync(post.IdUser, "USER");
                    if (!string.IsNullOrEmpty(avatarUser))
                    {
                        postResponse.UserOfPostResponse.UrlAvatar = avatarUser;
                    }

                    // Snapshot
                    if (!string.IsNullOrEmpty(post.Snapshot))
                    {
                        try
                        {
                            var snapshot = JsonSerializer.Deserialize<SnapshotResponse>(
                                post.Snapshot,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );
                            postResponse.SnapshotResponse = snapshot != null ? new List<SnapshotResponse> { snapshot } : null;
                        }
                        catch (JsonException)
                        {
                            postResponse.SnapshotResponse = null;
                        }
                    }

                    if (imagesDict.TryGetValue(post.IdPost, out var imageUrl))
                    {
                        postResponse.UrlImage = imageUrl;
                    }

                    listResponse.Add(postResponse);
                }

                return ApiResponse<List<PostResponse>>.SuccessResponse("Lấy danh sách gói thành công!", 200, listResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PostResponse>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<string>> CreateTransactionPostAsync(TransactionPostCreationRequest transactionPostCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(transactionPostCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                var postDomain = _mapper.Map<PostDomain>(transactionPostCreationRequest);

                // Chuyển đổi thành Json để lưu
                if (transactionPostCreationRequest.TransactionOfPost != null && transactionPostCreationRequest.TransactionOfPost.Any())
                {
                    var snapshot = new SnapshotResponse
                    {
                        TransactionOfPosts = transactionPostCreationRequest.TransactionOfPost,
                        InvestmentAssetOfPosts = null
                    };
                    postDomain.Snapshot = JsonSerializer.Serialize(snapshot);
                }

                var createdPost = await _postRepository.AddPostAsync(postDomain);

                if (!string.IsNullOrEmpty(transactionPostCreationRequest.UrlImage))
                {
                    var image = new ImageDomain
                    {
                        Url = transactionPostCreationRequest.UrlImage,
                        IdRef = createdPost.IdPost,
                        RefType = "POSTS"
                    };
                    await _imageRepository.AddImageAsync(image);
                }

                return ApiResponse<string>.SuccessResponse("Tạo bài đăng thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> CreateInvestmentAssetPostAsync(InvestmentAssetPostCreationRequest investmentAssetPostCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(investmentAssetPostCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<string>.FailResponse("Không tìm thấy người dùng!", 404);

                var postDomain = _mapper.Map<PostDomain>(investmentAssetPostCreationRequest);

                // Chuyển đổi thành Json để lưu
                if (investmentAssetPostCreationRequest.InvestmentAssetOfPost != null && investmentAssetPostCreationRequest.InvestmentAssetOfPost.Any())
                {
                    var snapshot = new SnapshotResponse
                    {
                        TransactionOfPosts = null,
                        InvestmentAssetOfPosts = investmentAssetPostCreationRequest.InvestmentAssetOfPost
                    };
                    postDomain.Snapshot = JsonSerializer.Serialize(snapshot);
                }
                var createdPost = await _postRepository.AddPostAsync(postDomain);

                if (!string.IsNullOrEmpty(investmentAssetPostCreationRequest.UrlImage))
                {
                    var image = new ImageDomain
                    {
                        Url = investmentAssetPostCreationRequest.UrlImage,
                        IdRef = createdPost.IdPost,
                        RefType = "POSTS"
                    };
                    await _imageRepository.AddImageAsync(image);
                }

                return ApiResponse<string>.SuccessResponse("Tạo bài đăng thành công!", 201, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeletePostAsync(Guid idPost)
        {
            try
            {
                await _postRepository.DeletePostAsync(idPost);
                var existingImageUrl = await _imageRepository.GetImageUrlByIdRefAsync(idPost, "POSTS");
                if (!string.IsNullOrEmpty(existingImageUrl))
                {
                    await _imageRepository.DeleteImageByIdRefAsync(idPost, "POSTS");
                }

                return ApiResponse<string>.SuccessResponse("Xóa bài đăng thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }
    }
}
