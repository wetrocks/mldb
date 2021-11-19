using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal class ReferenceDataLoader {
    
        private const string SEED_LITTERTYPES_RESOURCE = "SeedData/seedLitterTypes.json";

        public static List<LitterTypesList> readLitterTypes() {
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using (var resourceStream = embeddedProvider.GetFileInfo(SEED_LITTERTYPES_RESOURCE).CreateReadStream())
            {
                return readLitterTypesFromJSON(resourceStream);
            }
        }

        private static List<LitterTypesList> readLitterTypesFromJSON(Stream jsonStream) {
            var typesList = new List<LitterTypesList>();
            using (StreamReader r = new StreamReader(jsonStream))
            {
                var json = r.ReadToEnd();
                typesList = JsonConvert.DeserializeObject<List<LitterTypesList>>(json);
            }

            return typesList;
        }
    }

}