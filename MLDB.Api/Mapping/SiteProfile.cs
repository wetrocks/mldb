using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Api.Models;

namespace MLDB.Api.Mapping {

    public class SiteProfile : Profile
    {
        public SiteProfile() {
            CreateMap<Site, SiteDTO>();
        }
    }
}