using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Api.Services
{
    public interface ILitterTypeRepository {

        public Task<IList<LitterType>> getAllAsync();
    }
}