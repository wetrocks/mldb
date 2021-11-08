using System;
using System.Collections.Generic;

namespace MLDB.Domain
{
    public class Site {

        public Guid Id{ get; init; }
        
        public string Name{ get; set; }

        public string CreateUserId{ get; init; }

        public  DateTime CreateTimestamp { get; init; }

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