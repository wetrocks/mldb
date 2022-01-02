using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using MLDB.Infrastructure.Repositories.Model;


namespace MLDB.Infrastructure.Repositories.DataLoad {
    internal class OsparMappingLoader : ResourceJsonDataLoader, ISeedDataLoader<LitterTypeMapping<UInt32, OsparLitterType>> {

       private const string INTERNALLITTERTYPES_RESOURCE = "SeedData/internalLitterTypes.json";

        public IReadOnlyList<LitterTypeMapping<UInt32, OsparLitterType>> readSeedData() {
            var mappings = new List<LitterTypeMapping<UInt32, OsparLitterType>>();
            using (StreamReader r = getResourceReader(INTERNALLITTERTYPES_RESOURCE))
            {
                var json = JObject.Parse(r.ReadToEnd());
                var readMappings = 
                    from t in json["osparTypeMapping"]
                    select new LitterTypeMapping<UInt32, OsparLitterType> { 
                        InternalLitterTypeCode = (UInt32) t["InternalLitterTypeCode"],
                        MappedTypeKey = (UInt32)t["OsparLitterTypeCode"]
                    };

                mappings = readMappings.ToList();
            }

            return mappings;
        }
    }
}