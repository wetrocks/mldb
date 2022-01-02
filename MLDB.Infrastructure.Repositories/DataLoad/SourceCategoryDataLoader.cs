using System.Collections.Generic;
using MLDB.Infrastructure.Repositories.Model;
using MLDB.Domain;

namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal class SourceCategoryDataLoader : ISeedDataLoader<LitterSourceCategory> {
        
        private static readonly List<LitterSourceCategory> SOURCE_CATEGORIES = new List<LitterSourceCategory>() {
            new LitterSourceCategory(1, "SUP", "Stand up paddleboards"),
            new LitterSourceCategory(2, "Fisheries", "Text about fisheries")
        };

        public IReadOnlyList<LitterSourceCategory> readSeedData() {
            return SOURCE_CATEGORIES.AsReadOnly();
        }
    }
}