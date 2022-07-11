using System;


namespace MLDB.Domain {
    public class User {

        public Int32 Id{ get; init; }

        public String IdpId { get; init; }

        public String Name { get; set; }

        public String Email{ get; set; }
        
        public Boolean EmailVerified{ get; set; }

        public DateTime CreateTimestamp { get; set; }

        public DateTime UpdateTimestamp { get; set; }


        public User(String IdpId) {
            this.IdpId = IdpId;

            this.CreateTimestamp = DateTime.UtcNow;
            this.UpdateTimestamp = this.CreateTimestamp;
        }
    }
}