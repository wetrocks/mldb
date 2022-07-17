using AutoMapper;
using MLDB.Api.DTO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using MLDB.Domain;

namespace MLDB.Api.Mapping
{

    public class SurveyProfile : Profile
    {
        public SurveyProfile()
        {
            CreateMap<Survey, SurveyDTO>()
                .ForMember(x => x.Coordinator, s => s.MapFrom(x => x.CoordinatorName))
                .ForMember(x => x.SurveyDate, s => s.MapFrom(x => x.SurveyDate.ToString("yyyy-MM-dd")))
                .ForMember(x => x.StartTime, s => s.MapFrom(x => x.StartTime.ToString("T", CultureInfo.InvariantCulture)))
                .ForMember(x => x.EndTime, s => s.MapFrom(x => x.EndTime.ToString("T", CultureInfo.InvariantCulture)))
                .ForMember(x => x.LitterItems,
                    s => s.MapFrom(x => x.LitterItems.ToDictionary(li => li.LitterTypeId, li => li.Count)));
        }
    }
}