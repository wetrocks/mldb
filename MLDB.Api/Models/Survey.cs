using System;


namespace MLDB.Api.Models
{
    public class Survey {

        public Guid Id{ get; set; }

        public string Coordinator{ get; set; }
        public string Date{ get; set; }

        public Int16 VolunteerCount{ get; set; }

        public string StartTime{ get; set; }

        public string EndTime{ get; set; }

        public Decimal TotalKg{ get; set; }
    }
}