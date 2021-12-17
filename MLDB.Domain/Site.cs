using System;
using System.Collections.Generic;

namespace MLDB.Domain
{
    public class Site {

        public Guid Id{ get; init; }
        
        public string Name{ get; set; }

        public string beachName{ get; set; }
        
        public string beachCode{ get; set; }

        public string countryCode{ get; set; }

        public string lowTideWidth{ get; set; }

        public string highTideWidth{ get; set; }

        public string beachLength{ get; set; }

        public string beachBack{ get; set; }

        public string startGPS{ get; set; }

        public string endGPS{ get; set; }

        public string quadrant1{ get; set; }

        public string quadrant2{ get; set; }

        public string quadrant3{ get; set; }

        public string coordinationSystem{ get; set; }

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
    
        public string CreateUserId{ get; init; }

        public  DateTime CreateTimestamp { get; init; }

        public Site( string name, string createUserId ) {
            Name = name;
            CreateUserId = createUserId;
            CreateTimestamp = DateTime.UtcNow;
        }

        public Site( Guid Id, string name, string createUserId ) : this(name, createUserId) {
            this.Id = Id;
        }
    }
}