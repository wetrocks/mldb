using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal class ResourceJsonDataLoader {

        public StreamReader getResourceReader(string resourceFile) {
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            var resourceStream = embeddedProvider.GetFileInfo(resourceFile).CreateReadStream();
            
            return new StreamReader(resourceStream);
        }

        public ResourceJsonDataLoader() {}
    }
}