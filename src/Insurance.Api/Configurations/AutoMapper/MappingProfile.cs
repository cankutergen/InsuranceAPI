using AutoMapper;
using Insurance.Api.Models;
using Insurance.Entities.Concrete;
using System.Linq;

namespace Insurance.Api.Configurations.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InsuranceModel, InsuranceResponseModel>();

            CreateMap<InsuranceOrder, InsuranceOrderResponseModel>()
                .ForMember(x => x.productIdList, opt => opt.MapFrom(s => s.InsuranceList.Select(x => x.ProductId)));
        }
    }
}
