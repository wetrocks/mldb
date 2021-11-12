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
            CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("s"));
            CreateMap<string, DateTime>().ConvertUsing(s => DateTime.ParseExact(s, "s", CultureInfo.InvariantCulture));

            CreateMap<Survey, SurveyDTO>()
                .ForMember(x => x.Coordinator, s => s.MapFrom(x => x.CoordinatorName))
                .ForMember(x => x.SurveyDate, s => s.MapFrom(x => x.StartTimeStamp.ToString("yyyy-MM-dd")))
                .ForMember(x => x.StartTime, s => s.MapFrom(x => x.StartTimeStamp.ToString("T", CultureInfo.InvariantCulture)))
                .ForMember(x => x.EndTime, s => s.MapFrom(x => x.EndTimeStamp.ToString("T", CultureInfo.InvariantCulture)))
                .ForMember(x => x.LitterItems,
                    s => s.MapFrom(x => x.LitterItems.ToDictionary(li => li.LitterTypeId, li => li.Count)));

            CreateMap<SurveyDTO, Survey>()
                .ConstructUsing( x => new Survey(x.Id, new List<int> { 42, 43 }, "testuser"))
                .ForCtorParam("siteId", s => s.MapFrom(x => x.Id))
                .ForCtorParam("createUserId", s => s.MapFrom(x => x.Coordinator))
                .ForMember(x => x.CoordinatorName, s => s.MapFrom(x => x.Coordinator))
                .ForMember( x => x.StartTimeStamp, s => s.MapFrom( x => $"{x.SurveyDate}T{x.StartTime ?? "00:00:00"}" ))
                .ForMember( x => x.EndTimeStamp, s => s.MapFrom( x => $"{x.SurveyDate}T{x.EndTime ?? "00:00:00"}" ))
                .ForMember( x => x.CreateUserId, s => s.Ignore())
                .ForMember( x => x.CreateTimestamp, s => s.Ignore())
                .ForMember( x => x.LitterItems, s => s.Ignore())
                .AfterMap((src, dest) => { 
                    src.LitterItems.AsEnumerable().ToList().ForEach( x => 
                        dest.updateLitterCount(int.Parse(x.Key), x.Value));
                });
        }
    }
}