using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Domain
{
    public interface ISiteRepository
    {

        public Task<IList<Site>> getAll();

        public Task<bool> existsAsync(Guid id);

        public Task<Site> findAsync(Guid id);

        public Task<Site> insertAsync(Site site);

        public Task<Site> updateAsync(Site site);

    }
}