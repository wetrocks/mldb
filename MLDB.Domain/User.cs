namespace MLDB.Domain;

using System;

public class User {

    public Int32 Id{ get; init; }

    public String IdpId { get; init; }

    public String Name { get; set; }

    public String Email{ get; set; }
    
    public Boolean EmailVerified{ get; set; }

    private DateTime _createTimestamp = DateTime.MinValue.ToUniversalTime();
    public DateTime CreateTimestamp { 
        get => _createTimestamp; 
        set => _createTimestamp = PropertyEnforcer.EnforceUtc(value);
    }

    private DateTime _updateTimestamp = DateTime.MinValue.ToUniversalTime();
    public DateTime UpdateTimestamp { 
        get => _updateTimestamp; 
        set => _updateTimestamp = PropertyEnforcer.EnforceUtc(value);
    }

    public User(String IdpId) {
        this.IdpId = IdpId;

        this.CreateTimestamp = DateTime.UtcNow;
        this.UpdateTimestamp = this.CreateTimestamp;
    }
}