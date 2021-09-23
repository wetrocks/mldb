using System;
using System.ComponentModel.DataAnnotations;


namespace MLDB.Api.Models
{
    public class Survey {
        [Key]
        public Guid Id{ get; set; }

        public string Coordinator{ get; set; }

        public DateTime Date{ get; set; }

        public Int16 VolunteerCount{ get; set; }

        public string StartTime{ get; set; }

        public string EndTime{ get; set; }

        public Decimal TotalKg{ get; set; }

        public Guid SiteId{ get; set; }
        
        public User CreateUser{ get; set; }

        public  DateTime CreateTimestamp { get; set; }

    }
}