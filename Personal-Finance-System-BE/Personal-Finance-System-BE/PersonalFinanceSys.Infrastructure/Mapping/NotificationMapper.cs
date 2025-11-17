using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Response;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper() 
        {
            CreateMap<Notification, NotificationDomain>();

            CreateMap<NotificationDomain, Notification>();

            CreateMap<NotificationDomain, NotificationResponse>();

            CreateMap<NotificationRequest, NotificationDomain>();
        }
    }
}
