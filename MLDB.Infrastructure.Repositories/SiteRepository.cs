using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using  MLDB.Domain;
using System.Linq;

namespace MLDB.Infrastructure.Repositories
{
    public class SiteRepository : ISiteRepository
    {

        private readonly SiteSurveyContext _dbCtx;

        public SiteRepository(SiteSurveyContext dbContext) {
            _dbCtx = dbContext;
        }

        public async Task<IList<Site>> getAll()
        {
            return null;
        }

        public async Task<bool> existsAsync(Guid id)
        {
            return false;
        }

        public async Task<Site> findAsync(Guid id)
        {
            var site = await _dbCtx.Sites.FindAsync(id);

            return site;
        }

        public async Task<Site> insertAsync(Site site)
        {
            return null;
        }
        public async Task<Site> updateAsync(Site site)
        {
            return null;
        }
    }
}