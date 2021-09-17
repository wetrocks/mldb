using System;
using System.ComponentModel.DataAnnotations;

namespace MLDB.Api.Models
{
    public class User {

        public User(String IdpId) {
            this.IdpId = IdpId;
        }

        [Key]
        public String IdpId { get; set; }

        public String Name { get; set; }

        public String Email{ get; set; }
        
        public Boolean EmailVerified{ get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}