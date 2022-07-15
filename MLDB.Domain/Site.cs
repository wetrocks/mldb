using System;
using System.Collections.Generic;

namespace MLDB.Domain
{
    public class Site {

        public Guid Id{ get; init; }
        
        public string Name{ get; set; }

        public string CreateUserId{ get; init; }

        private DateTime _createTs;
        public  DateTime CreateTimestamp { 
            get => _createTs;
            init => _createTs = value.Kind == DateTimeKind.Utc ? value : throw new ArgumentException("Kind must be Utc");
        }

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