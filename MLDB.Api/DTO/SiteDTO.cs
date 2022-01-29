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

        public string nearestTown{ get; set; }
        public string townPosition{ get; set; }

        public string townPopulation{ get; set; }

        public Boolean developmentBehindBeach{ get; set; }

        public string developmentBehindBeachDesc{ get; set; }

        public Boolean foodOnBeach{ get; set; }

        public string foodDistance{ get; set; }

        public Boolean foodYearRound{ get; set; }

        public string foodMonths{ get; set; }

        public string foodPosition{ get; set; }

        public string nearestShippingLane{ get; set; }

        public string shippingLaneDensity{ get; set; }

        public string shippingLaneUse{ get; set; }

        public string shippingLanePosition{ get; set; }

        public string nearestHarbour{ get; set; }

        public string harbourName{ get; set; }

        public string harbourType{ get; set; }

        public string harbourSize{ get; set; }

        public string harbourPosition{ get; set; }

        public string riverDistance{ get; set; }

        public string riverName{ get; set; }

        public string riverPosition{ get; set; }

        public string nearDischarge{ get; set; }

        public string dischargeDistance{ get; set; }

        public string dischargePosition{ get; set; }

        public string cleanedHowOften{ get; set; }

        public string cleanedMonths{ get; set; }

        public string cleanedOther{ get; set; }

        // public Boolean cleanedYearRoundOrSeasonal{ get; set; }

        public string cleaningMethod{ get; set; }
        
        public string responsibleForCleaning{ get; set; }

        public string additionalComments{ get; set; }

        public string amendment{ get; set; }

        public string dateCompleted{ get; set; }

        public string enteredBy{ get; set; }

        public string enteredByPhone{ get; set; }

        public string enteredByEmail{ get; set; }

        public string CreatedBy { get; init; }

        public List<SurveyDTO> Surveys { get; init; }
    };
}
