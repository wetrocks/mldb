using System;


namespace MLDB.Domain {
    public class User {

        public Int32 Id{ get; init; }

        public String IdpId { get; init; }

        public String Name { get; set; }

        public String Email{ get; set; }
        
        public Boolean EmailVerified{ get; set; }

        private DateTime _createTimestamp;
        public DateTime CreateTimestamp { 
            get => _createTimestamp; 
            set => _createTimestamp = value.Kind == DateTimeKind.Utc ? value : throw new ArgumentException("Kind must be Utc");
        }

        private DateTime _updateTimestamp;
        public DateTime UpdateTimestamp { 
            get => _updateTimestamp; 
            set => _updateTimestamp = value.Kind == DateTimeKind.Utc ? value : throw new ArgumentException("Kind must be Utc");
        }


        public User(String IdpId) {
            this.IdpId = IdpId;

            this.CreateTimestamp = DateTime.UtcNow;
            this.UpdateTimestamp = this.CreateTimestamp;
        }
    }
}