using System;


namespace MLDB.Domain {
    public class User {

        public User(String IdpId) {
            this.IdpId = IdpId;
        }

        public String IdpId { get; init; }

        public String Name { get; set; }

        public String Email{ get; set; }
        
        public Boolean EmailVerified{ get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}