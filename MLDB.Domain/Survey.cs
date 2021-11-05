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

        public Survey(Guid siteId, SurveyTemplate template, string createUserId) {            
            if( template == null || template.LitterTypes.Count == 0) {
                throw new ArgumentException("Survey template must not be null and contain at least one litter type");
            }
            
            SiteId = siteId;
            CreateUserId = createUserId;
            CreateTimestamp = DateTime.UtcNow;

            _litterItems.AddRange(template.LitterTypes.Select( x => new LitterItem(x.Id) ));
        }
    }
}