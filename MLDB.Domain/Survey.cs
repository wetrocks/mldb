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

        private  List<LitterItem> _litterItems = new List<LitterItem>();
        public IReadOnlyList<LitterItem> LitterItems =>  _litterItems;

        private Survey(Guid siteId, string createUserId) {
            SiteId = siteId;
            CreateUserId = createUserId;
            CreateTimestamp = DateTime.UtcNow;
        }

        public Survey(Guid siteId, IList<int> litterTypes, string createUserId) : this(siteId, createUserId) {            
            if( litterTypes == null || litterTypes.Count == 0) {
                throw new ArgumentException("Cannot create a survey without litterTypes");
            }
        
            _litterItems.AddRange(litterTypes.Select( x => new LitterItem(x) ));
        }

        public Survey(Guid id, Guid siteId, IList<int> litterTypes, string createUserId) : this(siteId, litterTypes, createUserId) {            
            this.Id = id;
        }

        public void updateLitterCount(int litterTypeId, int count) {
            var litterItem = _litterItems.Single( x => x.LitterTypeId == litterTypeId );
            litterItem.Count = count;
        }
    }
}