using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Api.Services
{
    public interface ISiteService {

        public Task<List<Site>> getAll();

        public Task<Site> find(Guid id);

        public Task<Site> create(Site site, ClaimsPrincipal userPrinciple);

    }
}