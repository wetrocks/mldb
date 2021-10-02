using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace MLDB.Api.Models
{
    public class Survey {
        [Key]
        public Guid Id{ get; set; }

        public string Coordinator{ get; set; }

        public DateTime StartTimeStamp{ get; set; }

        public DateTime EndTimeStamp{ get; set; }

        public Int16 VolunteerCount{ get; set; }

        public Decimal TotalKg{ get; set; }

        public Guid SiteId{ get; set; }
        
        public User CreateUser{ get; set; }

        public  DateTime CreateTimestamp { get; set; }

        public List<LitterItem> LitterItems { get; set; }
    }
}