using System;

namespace MLDB.Infrastructure.Repositories.Model {

    internal class LitterTypeMapping<K,T> {
      
        public UInt32 InternalLitterTypeCode{ get; init; }

        internal K MappedTypeKey { get; init; }

        public  T MappedType{ get; init; }
    }
}