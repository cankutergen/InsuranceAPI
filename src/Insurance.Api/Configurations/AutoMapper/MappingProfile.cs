using AutoMapper;
using Insurance.Api.Models;
using Insurance.Entities.ComplexTypes;
using Insurance.Entities.Concrete;
using System.Linq;

namespace Insurance.Api.Configurations.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InsuranceModel, InsuranceResponseModel>();

            CreateMap<OrderInsurance, OrderInsuranceResponseModel>();
                
        }
    }
}
