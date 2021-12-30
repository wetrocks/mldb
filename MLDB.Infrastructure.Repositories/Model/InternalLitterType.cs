using System;
using MLDB.Domain;

namespace MLDB.Infrastructure.Repositories.Model {

    internal class InternalLitterType : ILitterType {

        public const string CODESET = "INTERNAL";
        
        public UInt32 Code { get; init; }

        public string CodeSet => CODESET;

        public string Description { get; init; }

        public string getCodeAsStr() => Code.ToString();

        internal UInt16? SourceCategoryId { get; init; }
        public LitterSourceCategory SourceCategory { get; init; }

        public LitterTypeMapping<UInt32, OsparLitterType> OsparMapping { get; init; }

        public LitterTypeMapping<String, JointListLitterType> JointListMapping { get; init; }

        internal InternalLitterType() {}

        public InternalLitterType(UInt32 code,  string description) {
            this.Code = code;
            this.Description = description;
        }
    }

}