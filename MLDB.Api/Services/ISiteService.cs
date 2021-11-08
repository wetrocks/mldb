using MLDB.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Api.Services
{
    [Obsolete]
    public interface ISiteService {

        public Task<IList<Site>> getAll();

        public Task<Site> getSite(Guid id);

        public Task<Site> create(Site site, User user);

        public Task<Site> update(Site site, User user);

    }
}