using System;

namespace MLDB.Infrastructure.Repositories.Model {

    internal class JointListLitterType : ILitterType {

        public string J_Code { get; set; }

        public const string CODESET = "JointList";
        
        public string TypeCode { get; init;}

        public string CodeSet => CODESET;

        public string Description { get; init; }
        
        public string Definition{ get; init; }

        public string getCodeAsStr() => TypeCode;
    }

}