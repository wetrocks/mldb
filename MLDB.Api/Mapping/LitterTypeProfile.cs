using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Domain;


namespace MLDB.Api.Mapping {

    public class LitterTypeProfile : Profile
    {
        public LitterTypeProfile() {
            CreateMap<LitterType, LitterTypeDTO>()
                .ForMember( x => x.SourceCategory, 
                            s => s.MapFrom( x => x.SourceCategory != null ? x.SourceCategory.Name : null));
        }
    }
}