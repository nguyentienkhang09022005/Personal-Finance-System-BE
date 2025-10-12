using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Application.UseCases.InvestmentFund
{
    public class InvestmentFundHandler
    {
        private readonly IInvestmentFundRepository _investmentFundRepository;
        private readonly IMapper _mapper;

        public InvestmentFundHandler(IInvestmentFundRepository investmentFundRepository, IMapper mapper)
        {
            _investmentFundRepository = investmentFundRepository;
            _mapper = mapper;
        }

        // Hàm tạo quỹ cá nhân
        public async Task<ApiResponse<string>> CreateInvestmentHandleAsync(InvestmentFundRequest investmentFundRequest)
        {
            var investmentFundDomain = _mapper.Map<InvesmentFundDomain>(investmentFundRequest);
            await _investmentFundRepository.AddInvestmentAsync(investmentFundDomain);
            return ApiResponse<string>.SuccessResponse("Tạo quỹ thành công!", 200, string.Empty);
        }
    }
}
