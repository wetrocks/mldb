using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using MLDB.Infrastructure.Repositories.Model;


namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal class OsparLitterTypeDataLoader : ResourceJsonDataLoader, ISeedDataLoader<OsparLitterType> {
        
        private const string OSPARLITTERTYPES_RESOURCE = "SeedData/osparLitterTypes.json";

        public IReadOnlyList<OsparLitterType> readSeedData() {
            var typesList = new List<OsparLitterType>();
            using (StreamReader r = getResourceReader(OSPARLITTERTYPES_RESOURCE))
            {
                var json = r.ReadToEnd();
                typesList = JsonConvert.DeserializeObject<List<OsparLitterType>>(json);
            }

            return typesList;
        }
    }
}