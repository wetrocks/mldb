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

        public string nearestTown{ get; set; }
        public string townPosition{ get; set; }

        public string townPopulation{ get; set; }

        public string behindBeachDev{ get; set; }

        public string foodOnBeach{ get; set; }

        public string foodDistance{ get; set; }

        public string foodYearRound{ get; set; }

        public string foodPosition{ get; set; }

        public string nearestShippingLane{ get; set; }

        public string shippingLaneDensity{ get; set; }

        public string shippingLaneUse{ get; set; }

        public string shippingLanePosition{ get; set; }

        public string nearestHarbour{ get; set; }

        public string harbourName{ get; set; }

        public string harbourType{ get; set; }

        public string harbourSize{ get; set; }

        public string riverDistance{ get; set; }

        public string riverName{ get; set; }

        public string riverPosition{ get; set; }

        public string nearDischarge{ get; set; }

        public string dischargeDistance{ get; set; }

        public string dischargePosition{ get; set; }

        public string cleanedHowOften{ get; set; }

        public string cleanedYearRoundOrSeasonal{ get; set; }

        public string cleaningMethod{ get; set; }
        
        public string responsibleForCleaning{ get; set; }

        public string additionalComments{ get; set; }

        public string amendment{ get; set; }

        public string dateCompleted{ get; set; }

        public string enteredBy{ get; set; }

        public string enteredByPhone{ get; set; }

        public string enteredByEmail{ get; set; }

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