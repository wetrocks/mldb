namespace MLDB.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

public class Survey {
    public Guid Id{ get; init; }

    public Guid SiteId{ get; init; }

    public string CreateUserId{ get; init; }

    private DateTime _createTs = DateTime.MinValue.ToUniversalTime();
    public  DateTime CreateTimestamp { 
        get => _createTs;
        init => _createTs = PropertyEnforcer.EnforceUtc(value);
    }
    
    public string CoordinatorName{ get; set; }

    private DateTime _startTs = DateTime.MinValue.ToUniversalTime();
    public DateTime StartTimeStamp { 
        get => _startTs;
        set => _startTs = PropertyEnforcer.EnforceUtc(value);
    }

    private DateTime _endTs = DateTime.MinValue.ToUniversalTime();
    public DateTime EndTimeStamp{ 
        get => _endTs;
        set => _endTs = PropertyEnforcer.EnforceUtc(value);
    }

    public Int16 VolunteerCount{ get; set; }

    public Decimal TotalKg{ get; set; }

    private HashSet<LitterItem> _litterItems = new HashSet<LitterItem>();
    public IReadOnlySet<LitterItem> LitterItems => _litterItems;

    public Survey(Guid siteId, string createUserId) {
        SiteId = siteId;
        CreateUserId = createUserId;
        CreateTimestamp = DateTime.UtcNow;
    }

    public Survey(Guid siteId, Guid surveyId, string createUserId) : this(siteId, createUserId) {
        Id = surveyId;
    }

    public void updateLitterItems(IEnumerable<LitterItem> litterItems) {
        var itemSet = litterItems.ToHashSet();

        var dups = itemSet.GroupBy( x => x.LitterTypeId )
                            .Where( g => g.Count() > 1)
                            .Select( g => g.Key );

        if( dups.Count() > 0) {
            throw new ArgumentException($"Parameter contained duplicate litter type ids {String.Join(",", dups)}");
        }

        _litterItems = litterItems.ToHashSet();
    }
}
