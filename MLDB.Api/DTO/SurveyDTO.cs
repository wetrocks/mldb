using System;
using System.Collections.Generic;

namespace MLDB.Api.DTO {

    public record SurveyDTO {
        public Guid Id{ get; init; }

        public string SurveyDate{ get; init; }

        public string StartTime{ get; init; }

        public string EndTime{ get; init; }

        public string Coordinator{ get; init; }

        public Int16 VolunteerCount{ get; init; }

        public Decimal TotalKg{ get; set; }

        public IDictionary<String, int> LitterItems{ get; init; } = new Dictionary<String, int>();
    }
}