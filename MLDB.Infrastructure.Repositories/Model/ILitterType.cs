using System;

namespace MLDB.Infrastructure.Repositories.Model {

    public interface ILitterType {
      
        string CodeSet { get; }
      
        string Description { get; init; }

        string getCodeAsStr();
    }

}