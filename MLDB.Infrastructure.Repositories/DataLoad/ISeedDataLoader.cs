using System.Collections.Generic;

namespace MLDB.Infrastructure.Repositories.DataLoad {

    internal interface ISeedDataLoader<T> {
        IReadOnlyList<T>  readSeedData();
    }
}