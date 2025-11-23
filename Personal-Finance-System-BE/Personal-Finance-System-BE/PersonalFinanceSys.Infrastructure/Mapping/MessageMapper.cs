using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<Message, MessageDomain>();

            CreateMap<MessageDomain, Message>()
                .ForMember(dest => dest.IdMessage, opt => opt.Ignore());

            CreateMap<MessageRequest, MessageDomain>();

            CreateMap<MessageDomain, MessageResponse>();
        }
    }
}
