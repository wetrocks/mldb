using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using MLDB.Infrastructure.Repositories.Model;


namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal class JointListLitterTypeDataLoader : ResourceJsonDataLoader, ISeedDataLoader<JointListLitterType> {
        
        private const string J_LISTLITTERTYPES_RESOURCE = "SeedData/jlistLitterTypes.json";

        public IReadOnlyList<JointListLitterType> readSeedData() {
            var typesList = new List<JointListLitterType>();
            using (StreamReader r = getResourceReader(J_LISTLITTERTYPES_RESOURCE))
            {
                var json = r.ReadToEnd();
                typesList = JsonConvert.DeserializeObject<List<JointListLitterType>>(json);
            }

            return typesList;
        }
    }
}