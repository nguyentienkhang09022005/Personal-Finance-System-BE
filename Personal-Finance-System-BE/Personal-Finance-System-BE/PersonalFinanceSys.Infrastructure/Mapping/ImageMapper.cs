using AutoMapper;
using Personal_Finance_System_BE.PersonalFinanceSys.Domain.Entities;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Mapping
{
    public class ImageMapper : Profile
    {
        public ImageMapper() 
        {
            CreateMap<ImageDomain, Image>();
        }
    }
}
