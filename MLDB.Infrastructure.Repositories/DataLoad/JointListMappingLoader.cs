using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using MLDB.Infrastructure.Repositories.Model;


namespace MLDB.Infrastructure.Repositories.DataLoad {
    internal class JointListMappingLoader : ResourceJsonDataLoader, ISeedDataLoader<LitterTypeMapping<String, JointListLitterType>> {

       private const string INTERNALLITTERTYPES_RESOURCE = "SeedData/internalLitterTypes.json";

        public IReadOnlyList<LitterTypeMapping<String, JointListLitterType>> readSeedData() {
            var mappings = new List<LitterTypeMapping<String, JointListLitterType>>();
            using (StreamReader r = getResourceReader(INTERNALLITTERTYPES_RESOURCE))
            {
                var json = JObject.Parse(r.ReadToEnd());
                var readMappings = 
                    from t in json["jlistTypeMapping"]
                    select new LitterTypeMapping<String, JointListLitterType> { 
                        InternalLitterTypeCode = (UInt32) t["InternalLitterTypeCode"],
                        MappedTypeKey = (String)t["JListLitterTypeCode"]
                    };

                mappings = readMappings.ToList();
            }

            return mappings;
        }
    }
}