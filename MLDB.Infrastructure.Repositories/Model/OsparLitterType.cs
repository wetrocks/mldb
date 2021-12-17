using System;

namespace MLDB.Infrastructure.Repositories.Model {

    internal class OsparLitterType : ILitterType {

        public const string CODESET = "OSPAR-LT";

        public string CodeSet => CODESET;

        public string Description { get; init; }

        public UInt32 Code { get; init; }

        public string Category { get; init; }

        public string getCodeAsStr() => Code.ToString();
    }

}