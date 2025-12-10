using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Constrant;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Repositories;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.Socials
{
    public class EvaluateHandler
    {
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public EvaluateHandler(IEvaluateRepository evaluateRepository, 
                               IUserRepository userRepository, 
                               IPostRepository postRepository, 
                               IImageRepository imageRepository,
                               IMapper mapper)
        {
            _evaluateRepository = evaluateRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EvaluateResponse>> CreateEvaluateAsync(EvaluateCreationRequest evaluateCreationRequest)
        {
            try
            {
                bool userExists = await _userRepository.ExistUserAsync(evaluateCreationRequest.IdUser);
                if (!userExists)
                    return ApiResponse<EvaluateResponse>.FailResponse("Không tìm thấy người dùng!", 404);

                bool postExists = await _postRepository.ExistPostAsync(evaluateCreationRequest.IdPost);
                if (!postExists)
                    return ApiResponse<EvaluateResponse>.FailResponse("Không tìm thấy bài đăng!", 404);

                var evaluateDomain = _mapper.Map<EvaluateDomain>(evaluateCreationRequest);
                var createdEvaluate = await _evaluateRepository.AddEvaluateAsync(evaluateDomain);

                var evaluateResponse = _mapper.Map<EvaluateResponse>(createdEvaluate);

                return ApiResponse<EvaluateResponse>.SuccessResponse("Tạo đánh giá thành công!", 201, evaluateResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluateResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<string>> DeleteEvaluateAsync(Guid idEvaluate)
        {
            try
            {
                await _evaluateRepository.DeleteEvaluteAsync(idEvaluate);
                return ApiResponse<string>.SuccessResponse("Xóa đánh giá thành công!", 200, string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<EvaluateResponse>> UpdateEvaluateAsync(Guid idEvaluate, EvaluateUpdateRequest evaluateUpdateRequest)
        {
            try
            {
                var evaluateEntity = await _evaluateRepository.GetExistEvaluate(idEvaluate);
                if (evaluateEntity == null){
                    return ApiResponse<EvaluateResponse>.FailResponse("Không tìm thấy đánh giá!", 404);
                }

                if (evaluateEntity.CreateAt < DateTime.UtcNow.AddMinutes(-30)){
                    return ApiResponse<EvaluateResponse>.SuccessResponse("Đã quá 30 phút, không thể chỉnh sửa đánh giá!", 400, null);
                }

                if (evaluateUpdateRequest.Star > 5 || evaluateUpdateRequest.Star < 1){
                    return ApiResponse<EvaluateResponse>.SuccessResponse("Số sao phải từ 1 - 5!", 400, null);
                }

                var evaluateDomain = _mapper.Map<EvaluateDomain>(evaluateEntity);

                _mapper.Map(evaluateUpdateRequest, evaluateDomain);
                     
                var updatedEvaluate = await _evaluateRepository.UpdateEvaluateAsync(evaluateDomain, evaluateEntity);

                var evaluateResponse = _mapper.Map<EvaluateResponse>(updatedEvaluate);
                return ApiResponse<EvaluateResponse>.SuccessResponse("Thay đổi đánh giá thành công!", 200, evaluateResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluateResponse>.FailResponse(ex.Message, 500);
            }
        }

        public async Task<ListEvaluateResponse> GetListEvaluateAsync(Guid idPost)
        {
            try
            {
                var checkPostExist = await _postRepository.ExistPostAsync(idPost);
                if (!checkPostExist){
                    throw new Exception("Không tìm thấy bài đăng!");
                }

                var evaluateDomains = await _evaluateRepository.GetListEvaluateByPostIdAsync(idPost);

                if (evaluateDomains == null || !evaluateDomains.Any())
                {
                    return new ListEvaluateResponse
                    {
                        TotalComments = 0,
                        AverageStars = 0,
                        EvaluateResponses = new List<EvaluateResponse>()
                    };
                }

                var evaluateResponses = new List<EvaluateResponse>();

                foreach (var evaluate in evaluateDomains)
                {
                    var avatarUser = await _imageRepository
                        .GetImageUrlByIdRefAsync(evaluate.IdUser, ConstantTypeRef.TypeUser);

                    var response = new EvaluateResponse
                    {
                        IdEvaluate = evaluate.IdEvaluate,
                        idUser = evaluate.IdUser,
                        Name = evaluate.IdUserNavigation?.Name,
                        UrlAvatar = avatarUser ?? string.Empty,
                        Star = evaluate.Star,
                        Comment = evaluate.Comment,
                        CreateAt = evaluate.CreateAt
                    };

                    evaluateResponses.Add(response);
                }

                var totalComments = evaluateResponses.Count;
                var averageStars = evaluateResponses.Average(e => e.Star ?? 0);

                var result = new ListEvaluateResponse
                {
                    TotalComments = totalComments,
                    AverageStars = Math.Round((decimal)averageStars, 2),
                    EvaluateResponses = evaluateResponses
                };

                return result;
            }
            catch{
                throw;
            }
        }
    }
}
