using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MLDB.Api.Models
{
    public class Site {
        [Key]
        public Guid Id{ get; set; }
        
        public string Name{ get; set; }

        public User CreateUser{ get; set; }

        public  DateTime CreateTimestamp { get; set; }

        public List<Survey> Surveys { get; set; }
    }
}