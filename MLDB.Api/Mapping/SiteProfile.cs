using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Api.Models;


namespace MLDB.Api.Mapping {

    public class SiteProfile : Profile
    {
        public SiteProfile() {
            CreateMap<Survey, SurveyDTO>()
                .ForMember( x => x.SurveyDate, s => s.MapFrom( s => s.StartTimeStamp.ToString("yyyy-MM-dd")) )
                .ForMember( x => x.LitterItems, opt => opt.Ignore() );


            CreateMap<Site, SiteDTO>()
                .ForMember( x => x.CreatedBy, s => s.MapFrom( x => x.CreateUser != null ? x.CreateUser.Name : null ))                  
                .ReverseMap()
                .ForMember( x => x.Surveys, opt => opt.Ignore() );

            CreateMap<MLDB.Domain.Site, SiteDTO>()
                    .ReverseMap()
                    .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                    .ForCtorParam("createUserId", opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}