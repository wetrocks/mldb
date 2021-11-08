using MLDB.Api.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace MLDB.Api.Services
{
    [Obsolete]
    public class SiteService : ISiteService {

        private readonly SiteSurveyContext _dbCtx;
        private readonly IUserService _userSvc;

        public SiteService(SiteSurveyContext context, IUserService userService) { 
            _dbCtx = context;
            _userSvc = userService;
        }

        public SiteService(SiteSurveyContext context) { 
            _dbCtx = context;
        }

        public async Task<IList<Site>> getAll() {
            return await _dbCtx.Sites.ToListAsync();
        }

        public async Task<Site> getSite(Guid id) {
            return await _dbCtx.Sites.Include( x => x.Surveys ).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Site> create(Site site, User user) {
            site.CreateUser = user;
            site.CreateTimestamp = System.DateTime.UtcNow;

            _dbCtx.Sites.Add(site);

            await _dbCtx.SaveChangesAsync();

            return site;
        }

        public async Task<Site> update(Site site, User user) {
            var existingSite = await _dbCtx.Sites.FindAsync(site.Id);
            if( existingSite == null ) {
                // TODO: throw something better
                throw new System.Data.RowNotInTableException();
            }

            existingSite.Name = site.Name;

            await _dbCtx.SaveChangesAsync();

            return existingSite;
        }
    }
}