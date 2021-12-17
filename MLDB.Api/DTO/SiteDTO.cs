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
        public string lowTideWidth { get; init; }
        public string highTideWidth { get; init; }
        public string beachLength { get; init; }
        public string beachBack { get; init; }
        public string startGPS { get; init; }
        public string endGPS { get; init; }
        public string quadrant1 { get; init; }
        public string quadrant2 { get; init; }
        public string quadrant3 { get; init; }
        public string coordinationSystem { get; init; }
        public string dateGPSPositionMeasured{ get; set; }

        public string prevailingCurrent{ get; set; }

        public string prevailingWinds{ get; set; }

        public string beachDirection{ get; set; }

        public string beachMaterial{ get; set; }

        public string beachTopography{ get; set; }

        public string beachCurvature{ get; set; }

        public string horizontalBeachProfile{ get; set; }

        public string seaObjects{ get; set; }

        public string majorUsage{ get; set; }

        public string beachVisitsPerYear{ get; set; }

        public string beachAccess{ get; set; }
    
        public string CreatedBy { get; init; }
        public List<SurveyDTO> Surveys { get; init; }
    };
}
