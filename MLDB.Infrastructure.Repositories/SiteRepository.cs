using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using  MLDB.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            return await _dbCtx.Sites.ToListAsync();
        }

        public bool exists(Guid id)
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
            var created = await _dbCtx.Sites.AddAsync(site);

            return created.Entity;
        }
        public async Task<Site> updateAsync(Site site)
        {
            var orig = await _dbCtx.Sites.FindAsync(site.Id);
            if( orig == null ) {
                // TODO: maybe throw something better?
                throw new System.Data.RowNotInTableException();
            }
            orig.Name = site.Name;

            var updated = _dbCtx.Sites.Update(orig);
            return updated.Entity;
        }
    }
}