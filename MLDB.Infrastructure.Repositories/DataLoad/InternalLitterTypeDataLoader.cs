using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using MLDB.Infrastructure.Repositories.Model;
using MLDB.Domain;

namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal class InternalLitterTypeDataLoader : ResourceJsonDataLoader, ISeedDataLoader<InternalLitterType> {
        private const string INTERNALLITTERTYPES_RESOURCE = "SeedData/internalLitterTypes.json";
        
        private Dictionary<string, LitterSourceCategory> categoryMap;
        
        public InternalLitterTypeDataLoader(IReadOnlyList<LitterSourceCategory> sourceCategories) {
            this.categoryMap = sourceCategories.ToDictionary( x => x.Name );
        }

        public IReadOnlyList<InternalLitterType> readSeedData() {

            var typesList = new List<InternalLitterType>();
            using (StreamReader r = getResourceReader(INTERNALLITTERTYPES_RESOURCE))
            {
                var json = JObject.Parse(r.ReadToEnd());
                var internalTypes = 
                    from t in json["internalLitterTypes"]
                    select new InternalLitterType { 
                        Code = (UInt32) t["Code"],
                        Description = (string)t["Description"],
                        SourceCategoryId = 
                            categoryMap.GetValueOrDefault( (string)t["Category"] ?? "NEVERHAPPEN!!one" )?.Id
                    };

                typesList = internalTypes.ToList();
            }

            return typesList;
        }
    }
}