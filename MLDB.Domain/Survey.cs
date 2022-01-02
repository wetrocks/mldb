using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace MLDB.Domain
{
    public class Survey {
        public Guid Id{ get; init; }

        public Guid SiteId{ get; init; }

        public string CreateUserId{ get; init; }

        public  DateTime CreateTimestamp { get; init; }
        
        public string CoordinatorName{ get; set; }

        public DateTime StartTimeStamp{ get; set; }

        public DateTime EndTimeStamp{ get; set; }

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
}