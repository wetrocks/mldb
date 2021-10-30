using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MLDB.Api.Services
{
    public interface ILitterTypeService {

        public Task<IList<LitterType>> getLitterTypesForVersion(Decimal version);
    }
}