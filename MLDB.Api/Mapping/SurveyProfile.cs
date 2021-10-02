using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MLDB.Api.Mapping {

    public class SurveyProfile : Profile
    {
        public SurveyProfile() {
            CreateMap<Survey, SurveyDTO>()
            .ForMember( x => x.LitterItems, s => s.MapFrom( x => x.LitterItems.ToDictionary( li => li.LitterType.Id.ToString(),
                                                                                             li => li.Count )))
            .ForMember( x => x.SurveyDate, s => s.MapFrom( x => x.StartTimeStamp.ToString("yyyy-MM-dd")))                                                                                
            .ForMember( x => x.StartTime, s => s.MapFrom( x => x.StartTimeStamp.ToString("T")))
            .ForMember( x => x.EndTime, s => s.MapFrom( x => x.EndTimeStamp.ToString("T")))
            .ReverseMap()
            .ForMember( x => x.StartTimeStamp, s => s.MapFrom( x => $"{x.SurveyDate}T{x.StartTime}" ))
            .ForMember( x => x.EndTimeStamp, s => s.MapFrom( x => $"{x.SurveyDate}T{x.EndTime}" ))
            .ForMember( x => x.LitterItems, s => s.MapFrom( x => x.LitterItems.Select(
                item => new LitterItem { SurveyId = x.Id, LitterTypeId = Int16.Parse(item.Key), Count = item.Value }
            )));
        }
    }
}