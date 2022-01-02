using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Domain
{
    public interface ILitterTypeRepository
    {

        public Task<List<LitterType>> getAll();

        public Task<LitterType> findAsync(UInt32 id);
    }
}