using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Domain;


namespace MLDB.Api.Mapping {

    public class SiteProfile : Profile
    {
        public SiteProfile() {
            CreateMap<Site, SiteDTO>()
                    .ForMember( x => x.CreatedBy, s => s.MapFrom( x => x.CreateUserId ))
                    .ReverseMap()
                    .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                    .ForCtorParam("createUserId", opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}