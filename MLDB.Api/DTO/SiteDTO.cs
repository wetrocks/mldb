using System;
using System.Collections.Generic;

namespace MLDB.Api.DTO {
    public record SiteDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string beachName { get; init; }
        public string beachCode { get; init; }
        public string countryCode { get; init; }
        public string CreatedBy { get; init; }
        public List<SurveyDTO> Surveys { get; init; }
    };
}
