namespace MLDB.Domain;

using System;

public class Site {

    public Guid Id{ get; init; }
    
    public string Name{ get; set; }

    public string CreateUserId{ get; init; }

    private DateTime _createTs = DateTime.MinValue.ToUniversalTime();
    public  DateTime CreateTimestamp { 
        get => _createTs;
        init => _createTs = PropertyEnforcer.EnforceUtc(value);
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
