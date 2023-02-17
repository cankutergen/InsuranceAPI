using AutoMapper;
using Insurance.Api.Models;
using Insurance.Entities.Concrete;

namespace Insurance.Api.Configurations.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InsuranceModel, InsuranceApiModel>();
        }
    }
}
