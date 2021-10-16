using AutoMapper;
using MLDB.Api.DTO;
using MLDB.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace MLDB.Api.Mapping {

    public class SurveyProfile : Profile
    {
        public SurveyProfile() {
            CreateMap<Survey, SurveyDTO>()
            .ForMember( x => x.LitterItems, s => s.MapFrom( x => x.LitterItems.ToDictionary( li => li.LitterTypeId,
                                                                                             li => li.Count )))
            .ForMember( x => x.SurveyDate, s => s.MapFrom( x => x.StartTimeStamp.ToString("yyyy-MM-dd")))                                                                                
            .ForMember( x => x.StartTime, s => s.MapFrom( x => x.StartTimeStamp.ToString("T", CultureInfo.InvariantCulture)))
            .ForMember( x => x.EndTime, s => s.MapFrom( x => x.EndTimeStamp.ToString("T", CultureInfo.InvariantCulture)))
            .ReverseMap()
            .ForMember( x => x.StartTimeStamp, s => s.MapFrom( x => $"{x.SurveyDate}T{x.StartTime ?? "00:00:00"}" ))
            .ForMember( x => x.EndTimeStamp, s => s.MapFrom( x => $"{x.SurveyDate}T{x.EndTime ?? "00:00:00"}" ))
            .ForMember( x => x.LitterItems, s => s.MapFrom( x => x.LitterItems.Select(
                item => new LitterItem { SurveyId = x.Id, LitterTypeId = Int16.Parse(item.Key), Count = item.Value }
            )));

            CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("s"));
            CreateMap<string, DateTime>().ConvertUsing(s => DateTime.ParseExact(s, "s", CultureInfo.InvariantCulture));
        }
    }
}